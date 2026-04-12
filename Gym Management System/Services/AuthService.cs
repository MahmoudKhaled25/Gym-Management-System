using Gym_Management_System.Abstractions;
using Gym_Management_System.Abstractions.Consts;
using Gym_Management_System.Authentication;
using Gym_Management_System.Contracts.Auth;
using Gym_Management_System.Errors;
using Mapster;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Gym_Management_System.Services;

public class AuthService(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider,SignInManager<ApplicationUser> signInManager,ILogger<AuthService> logger) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        // check if the email is correct
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
        // check if the password is correct
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false,false);


        if (result.Succeeded)
        {
            var roles =  await _userManager.GetRolesAsync(user);
            var (token, expiresIn) = _jwtProvider.GenerateToken(user, roles);

            var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expiresIn, roles);
            return Result.Success(response);
        }
        var error = result.IsLockedOut ? UserErrors.LockedUser : UserErrors.InvalidCredentials;
        return Result.Failure<AuthResponse>(error);
    }

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.Email;

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        await _userManager.AddToRoleAsync(user, DefaultRoles.Member.Name);

        return Result.Success();
    }


    public async Task<Result> SendResetPasswordCodeAsync(ForgetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

        //if (!user.EmailConfirmed)
        //    return Result.Failure(UserErrors.EmailNotConfirmed with { StatusCode = StatusCodes.Status400BadRequest });

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Password reset code generated for user {Email}: {Code}", user.Email, code);

        // to do : send the code to the user's email using an email service
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
}
