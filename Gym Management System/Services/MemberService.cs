using Gym_Management_System.Abstractions;
using Gym_Management_System.Abstractions.Consts;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Member;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;

namespace Gym_Management_System.Services;

public class MemberService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,ApplicationDbContext context) : IMemberService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ApplicationDbContext _context = context;

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

    public async Task<Result<UserProfileResponse>> AddMemberAsync(AddMemberRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Weight = request.Weight,
                Height = request.Height,
                PhoneNumber = request.PhoneNumber 
            };
             
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Any(e => e.Code == "DuplicateEmail")
                            ? UserErrors.DuplicatedEmail
                            : UserErrors.InvalidCredentials;
                return Result.Failure<UserProfileResponse>(error);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Member.Name);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure<UserProfileResponse>(UserErrors.InvalidRoles);

            }
                        await transaction.CommitAsync(cancellationToken);

            var response = new UserProfileResponse(
                user.Id,
                user.Email!,
                user.FirstName,
                user.LastName,
                user.DateOfBirth,
                user.Weight,
                user.Height,
                null,
                new List<string> { DefaultRoles.Member.Name }
            );
             return Result.Success(response);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<UserProfileResponse>(UserErrors.InvalidCredentials);
        }

    }

    public async Task<Result> ToggleStatusAsync(string memberId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(memberId);

        if (user == null)
            return Result.Failure(UserErrors.UserNotFound);

        if (user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow)
        {
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        }
        else
        {
            user.LockoutEnd = null;
        }

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(UserErrors.UpdateFailed);
    }
}
