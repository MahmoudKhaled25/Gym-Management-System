using Gym_Management_System.Abstractions;
using Gym_Management_System.Abstractions.Consts;
using Gym_Management_System.Authentication;
using Gym_Management_System.Contracts.Auth;
using Gym_Management_System.Errors;
using Mapster;

namespace Gym_Management_System.Services;

public class AuthService(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider,SignInManager<ApplicationUser> signInManager) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

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
}
