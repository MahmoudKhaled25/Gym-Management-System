using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Enums;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using GymManagementSystem.Contracts.Admin;
using GymManagementSystem.Contracts.Dashboard;
using GymManagementSystem.Enums;

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
    public async Task<Result<DashboardResponse>> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var totalMembers = await _context.Users
          .Where(u => _context.UserRoles
              .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
              .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name))
          .CountAsync(cancellationToken);

        var activeMembers = await _context.Users
          .Where(u => _context.UserRoles
              .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
              .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name)
              && (u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow))
          .CountAsync(cancellationToken);

        var totalTrainers = await _context.Trainers.CountAsync(cancellationToken);
        var activeTrainers = await _context.Trainers
          .Where(t => t.IsActive)
          .CountAsync(cancellationToken);

        var totalSubscriptions = await _context.Subscriptions.CountAsync(cancellationToken);
        var activeSubscriptions = await _context.Subscriptions
          .Where(s => s.Status == SubscriptionStatus.Active)
          .CountAsync(cancellationToken);

        var pendingRequests = await _context.SubscriptionRequests
            .Where(r => r.Status == SubscriptionRequestStatus.Pending)
            .CountAsync(cancellationToken);

        var totalRevenue = await _context.Subscriptions
                 .Where(s => s.Status != SubscriptionStatus.Cancelled)
                 .SumAsync(s => s.MembershipPlan!.Price, cancellationToken);

        var subscriptionsByPlan = await _context.Subscriptions
            .Where(s => s.Status == SubscriptionStatus.Active)
                    .GroupBy(s => s.MembershipPlan!.Name)
                    .Select(g => new PlanSubscriptionsCount
                    (
                        g.Key,
                        g.Count()
                    ))
                    .ToListAsync(cancellationToken);

        var newMembersPerMonth = await _context.Users
            .Where(u => _context.UserRoles
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name))
            .GroupBy(x => x.CreatedAt.Month)
            .Select(g => new MonthlyCount
            (
                Month: g.Key.ToString(),
                Count: g.Count()
            )) .ToListAsync(cancellationToken);

        var revenuePerMonth = await _context.Subscriptions
        .Where(s => s.StartDate.Year == DateTime.UtcNow.Year
                 && s.Status != SubscriptionStatus.Cancelled)
        .GroupBy(s => s.StartDate.Month)
        .Select(g => new MonthlyRevenue(
            g.Key.ToString(),
            g.Sum(s => s.MembershipPlan!.Price)))
        .ToListAsync(cancellationToken);


        return Result.Success(new DashboardResponse(
       totalMembers,
       activeMembers,
       totalTrainers,
       activeTrainers,
       totalSubscriptions,
       activeSubscriptions,
       pendingRequests,
       totalRevenue,
       subscriptionsByPlan,
       newMembersPerMonth,
       revenuePerMonth
   ));
    }
 }
