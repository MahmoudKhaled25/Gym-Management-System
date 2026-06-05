using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using GymManagementSystem.Contracts.Admin;

namespace GymManagementSystem.Services;

public class AdminService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IAdminService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result> AddAdminAsync(AddAdminRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var admin = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender
            };
             var result = await _userManager.CreateAsync(admin, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Any(e => e.Code == "DuplicateEmail")
                    ? UserErrors.DuplicatedEmail
                    : UserErrors.InvalidCredentials;
                return Result.Failure(error);
            }
            var roleResult = await _userManager.AddToRoleAsync(admin, DefaultRoles.Admin.Name);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(UserErrors.InvalidRoles);
            }
            await transaction.CommitAsync(cancellationToken);
            return Result.Success();

        }
        catch 
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure(UserErrors.InvalidCredentials);
        }
    }
    public async Task<Result<IEnumerable<AdminResponse>>> GetAllAdminsAsync(CancellationToken cancellationToken = default)
    {
        var admins = await _context.Users
            .Where(u => _context.UserRoles
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
             .Any(ur => ur.UserId == u.Id && ur.Name == DefaultRoles.Admin.Name))
                            .AsNoTracking()
                            .Select(u => new AdminResponse
                            (
                            u.Id,
                            u.FirstName + " " + u.LastName,
                            u.Email!,
                            u.PhoneNumber,
                            u.Gender,
                            u.LockoutEnd <= DateTimeOffset.UtcNow || u.LockoutEnd == null ? true : false
                            ))
                            .ToListAsync(cancellationToken);

        return Result.Success(admins.AsEnumerable());
    }

    public async Task<Result> ToggleStatusAsync(string adminId, CancellationToken cancellationToken = default)
    {
        var admin = await _context.Users.SingleOrDefaultAsync(x => x.Id == adminId, cancellationToken);
        if (admin == null)
        {
            return Result.Failure(UserErrors.UserNotFound);
        }
        if (!await _userManager.IsInRoleAsync(admin, DefaultRoles.Admin.Name))
        {
            return Result.Failure(UserErrors.UserNotFound);
        }

        admin.LockoutEnd = admin.LockoutEnd <= DateTimeOffset.UtcNow || admin.LockoutEnd == null
            ? DateTimeOffset.UtcNow.AddYears(100)
            : null;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
