using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;

namespace Gym_Management_System.Services;

public interface IAccountService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default!);

    Task<Result> UpdateProfileAsync(string userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default);
}
