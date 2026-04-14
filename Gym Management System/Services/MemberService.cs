using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Member;
using Gym_Management_System.Errors;

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

    



    public async Task<Result<IEnumerable<UserProfileResponse>>> GetActiveMembersAsync()
    {
        var activeMembers = await _userManager.Users.Include(x => x.Trainer)
            .Where(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow)
            .ToListAsync();

        var response = new List<UserProfileResponse>();

        foreach(var activeMember in activeMembers)
        {
            var roles = await _userManager.GetRolesAsync(activeMember);
            response.Add(new UserProfileResponse(
                activeMember.Id,
                activeMember.Email!,
                activeMember.FirstName,
                activeMember.LastName,
                activeMember.DateOfBirth,
                activeMember.Weight,
                activeMember.Height,
                activeMember.Trainer == null ? null : new TrainerResponse(
                    activeMember.Trainer.ApplicationUser!.FirstName,
                    activeMember.Trainer.ApplicationUser.LastName,
                    activeMember.Trainer.Specialization,
                    activeMember.Trainer.IsActive
                ),
                roles
            ));

        }
        return Result.Success(response.AsEnumerable());

    }
    public async Task<Result<UserProfileResponse>> GetMembersAsync(string memberId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.Include(x => x.Trainer).FirstOrDefaultAsync(u => u.Id == memberId, cancellationToken);
        if (user is null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

       var response = new UserProfileResponse(
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
            await _userManager.GetRolesAsync(user)
        );
        return Result.Success(response);
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
