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
        var membersData = await _context.Users
            .Where(u => _context.UserRoles
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name }) 
                .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name)) 
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.DateOfBirth,
                u.Weight,
                u.Height,
                Roles = _context.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToList()
            })
            .ToListAsync();

        var response = membersData.Select(m => new UserProfileResponse(
            m.Id,
            m.Email!,
            m.FirstName,
            m.LastName,
            m.DateOfBirth,
            m.Weight,
            m.Height,
            m.Roles!
        ));

        return Result.Success(response.AsEnumerable());

    }

    public async Task<Result<IEnumerable<UserProfileResponse>>> GetActiveMembersAsync()
    {
        var membersData = await _context.Users
                    .Where(u => _context.UserRoles.Join(_context.Roles,ur => ur.RoleId ,r => r.Id,(ur,r) => new {ur.UserId,r.Name})
                                                   .Any(x => x.UserId == u.Id  && x.Name == DefaultRoles.Member.Name) && (u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow)) // to do : Make it works for subscription
                    .Select(u => new
            {
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.DateOfBirth,
                u.Weight,
                u.Height,
                Roles = _context.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToList()
            })
            .ToListAsync();

        var response = membersData.Select(m => new UserProfileResponse(
            m.Id,
            m.Email!,
            m.FirstName,
            m.LastName,
            m.DateOfBirth,
            m.Weight,
            m.Height,
            m.Roles!
        ));

        return Result.Success(response.AsEnumerable());

    }
    public async Task<Result<UserProfileResponse>> GetMemberAsync(string memberId, CancellationToken cancellationToken = default)
    {
        var memberData = await _context.Users
                   .Where(u => u.Id == memberId && 
                               _context.UserRoles
                                   .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                                   .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name))
                   .Select(u => new
                   {
                       u.Id,
                       u.Email,
                       u.FirstName,
                       u.LastName,
                       u.DateOfBirth,
                       u.Weight,
                       u.Height,
                       Roles = _context.UserRoles
                           .Where(ur => ur.UserId == u.Id)
                           .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                           .ToList()
                   })
                   .FirstOrDefaultAsync(cancellationToken); 

        if (memberData is null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

        var response = new UserProfileResponse(
            memberData.Id,
            memberData.Email!,
            memberData.FirstName,
            memberData.LastName,
            memberData.DateOfBirth,
            memberData.Weight,
            memberData.Height,
            memberData.Roles!
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
