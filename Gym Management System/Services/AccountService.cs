using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;

namespace Gym_Management_System.Services;

public class AccountService(ApplicationDbContext context,UserManager<ApplicationUser> userManager) : IAccountService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
                    .Include(u => u.Trainer)
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(user);
        var response = user.Adapt<UserProfileResponse>() with { Roles = roles };

        return Result.Success(response);

    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default)
    {

        var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Result.Failure(UserErrors.UserNotFound);

        request.Adapt(user);

        await _userManager.UpdateAsync(user);

        return Result.Success();

    }

}
