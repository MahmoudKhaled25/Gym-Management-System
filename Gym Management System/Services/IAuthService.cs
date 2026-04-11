using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Auth;

namespace Gym_Management_System.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request,CancellationToken cancellationToken = default!);
}
