using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Member;

namespace Gym_Management_System.Services;

public class MemberService(UserManager<ApplicationUser> userManager) : IMemberService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<IEnumerable<UserProfileResponse>>> GetAllMembersAsync()
    {
        var users = await _userManager.Users
        .Include(x => x.Trainer)
        .ToListAsync();

        var response = new List<UserProfileResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            response.Add(new UserProfileResponse(
                user.Id,
                user.Email!,
                user.FirstName,
                user.LastName,
                user.DateOfBirth,
                user.Weight,
                user.Height,
                user.Trainer == null ? null : new TrainerResponse(
                    user.Trainer.ApplicationUser!.FirstName,
                    user.Trainer.ApplicationUser.LastName,
                    user.Trainer.Specialization,
                    user.Trainer.IsActive
                ),
                roles
            ));
        }

        return Result.Success(response.AsEnumerable());
    }

    



    public Task<Result<IEnumerable<UserProfileResponse>>> GetActiveMembersAsync(string memberId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public Task<Result<UserProfileResponse>> GetMembersAsync(string memberId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UserProfileResponse>> AddMemberAsync(AddMemberRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ToggleStatusAsync(string memberId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
