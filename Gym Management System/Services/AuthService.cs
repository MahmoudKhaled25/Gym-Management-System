using Gym_Management_System.Abstractions;
using Gym_Management_System.Abstractions.Consts;
using Gym_Management_System.Authentication;
using Gym_Management_System.Contracts.Auth;
using Gym_Management_System.Errors;
using GymManagementSystem.Contracts.Auth;
using GymManagementSystem.Services;
using GymManagementSystem.Templates;
using Mapster;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace Gym_Management_System.Services;

public class AuthService(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider,SignInManager<ApplicationUser> signInManager,ILogger<AuthService> logger,IEmailService emailService) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Login attempt received");
        // check if the email is correct
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
        {
            _logger.LogWarning("Failed login attempt for {Email}", request.Email);
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
          
        }
        // check if the password is correct
      

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false,true);
        

        if (result.Succeeded)
        {
            var roles =  await _userManager.GetRolesAsync(user);
            var (token, expiresIn) = _jwtProvider.GenerateToken(user, roles);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });
            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expiresIn, roles,refreshToken,refreshTokenExpiration);
            _logger.LogInformation("User {UserId} logged in successfully", user.Id);
            return Result.Success(response);
        }
        
        var error = result.IsLockedOut ? UserErrors.LockedUser : !user.EmailConfirmed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials;

        if(error == UserErrors.LockedUser)
            _logger.LogWarning("User {Email} is locked out", request.Email);
        if(error == UserErrors.InvalidCredentials)
            _logger.LogWarning("Invalid login attempt for {Email}", request.Email);

        return Result.Failure<AuthResponse>(error);
    }


    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(request.Token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var user = await _userManager.Users
                  .Include(u => u.RefreshTokens)
                  .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        if(user.LockoutEnd is not null && user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

        if (user.LockoutEnd > DateTime.UtcNow)
          return Result.Failure<AuthResponse>(UserErrors.LockedUser);

        var userRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken && rt.IsActive);
        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;
        var roles = await _userManager.GetRolesAsync(user);
        var (token, expiresIn) = _jwtProvider.GenerateToken(user,roles );
        var newRefreshToken = GenerateRefreshToken();
        var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = newRefreshTokenExpiration
        });
                await _userManager.UpdateAsync(user);
        var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expiresIn,roles,newRefreshToken,newRefreshTokenExpiration);
        return Result.Success(response);

    }

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.Email;

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            _logger.LogInformation("Confirmation Code : {code}", encodedToken);
            var confirmationLink =
                        $"https://localhost:7088/api/auth/confirm-email" +
                        $"?email={Uri.EscapeDataString(user.Email!)}" +
                        $"&token={encodedToken}";
            await SendConfirmationEmail(user, confirmationLink);

            return Result.Success();    

        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> SendResetPasswordCodeAsync(ForgetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed with { StatusCode = StatusCodes.Status400BadRequest });

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Password reset code generated for user {Email}: {Code}", user.Email, code);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure(UserErrors.InvalidCredentials);

        IdentityResult identityResult;

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            identityResult = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);

        }
        catch
        {
            identityResult = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }
        if (identityResult.Succeeded)
            return Result.Success();
            
        var error = identityResult.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));

    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);
        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);
        var code = request.Token;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }


        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, DefaultRoles.Member.Name);
            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }


    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
    private async Task SendConfirmationEmail(ApplicationUser user, string confirmationLink)
    {
        var body = EmailTemplates.GetConfirmationEmailBody(user.FirstName, confirmationLink);

        await _emailService.SendEmailAsync(
            user.Email!,
            "Confirm your email - Gym Management",
            body);  
    }

    
}
