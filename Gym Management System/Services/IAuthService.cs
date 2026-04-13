using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Auth;

namespace Gym_Management_System.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request,CancellationToken cancellationToken = default!);

    // to do : add refresh token method
    Task<Result<AuthResponse>> GetRefreshTokenAsync(RefreshTokenRequest request,CancellationToken cancellationToken = default!);

    Task<Result> RegisterAsync(RegisterRequest request,CancellationToken cancellationToken = default!);

    Task<Result> SendResetPasswordCodeAsync(ForgetPasswordRequest request,CancellationToken cancellationToken = default!);

    Task<Result> ResetPasswordAsync (ResetPasswordRequest request, CancellationToken cancellationToken = default!);
}
