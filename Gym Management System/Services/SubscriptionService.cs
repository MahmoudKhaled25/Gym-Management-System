using Gym_Management_System.Contracts.Subscription;
using Gym_Management_System.Enums;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;

namespace Gym_Management_System.Services;

public class SubscriptionService(ApplicationDbContext context,UserManager<ApplicationUser> userManager) : ISubscriptionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    public async Task<Result<IEnumerable<SubscriptionResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var response = await _context.Subscriptions
    .Select(s => new SubscriptionResponse(
        s.Id,
        s.User!.Id,
        $"{s.User.FirstName} {s.User.LastName}",
        s.Trainer == null ? null : s.Trainer.UserId,
        s.Trainer == null ? null : $"{s.Trainer.ApplicationUser!.FirstName} {s.Trainer.ApplicationUser.LastName}",
        s.MembershipPlan!.Id,
        s.MembershipPlan.Name,
        s.StartDate,
        s.EndDate,
        s.Status
    ))
    .AsNoTracking()
    .ToListAsync(cancellationToken);
        return Result.Success(response.AsEnumerable());


    }
    public async Task<Result<IEnumerable<SubscriptionResponse>>> GetAllActiveAsync(CancellationToken cancellationToken)
    {
        var response = await _context.Subscriptions
            .Where(x => x.Status == SubscriptionStatus.Active)
            .Select(s => new SubscriptionResponse(
                s.Id,
                s.User!.Id,
                $"{s.User.FirstName} {s.User.LastName}",
                s.Trainer == null ? null : s.Trainer.UserId,
                s.Trainer == null ? null : $"{s.Trainer.ApplicationUser!.FirstName} {s.Trainer.ApplicationUser.LastName}",
                s.MembershipPlan!.Id,
                s.MembershipPlan.Name,
                s.StartDate,
                s.EndDate,
                s.Status
            ))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return Result.Success(response.AsEnumerable());
    }

    public async Task<Result<SubscriptionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _context.Subscriptions
           .Select(s => new SubscriptionResponse(
               s.Id,
               s.User!.Id,
               $"{s.User.FirstName} {s.User.LastName}",
               s.Trainer == null ? null : s.Trainer.UserId,
               s.Trainer == null ? null : $"{s.Trainer.ApplicationUser!.FirstName} {s.Trainer.ApplicationUser.LastName}",
               s.MembershipPlan!.Id,
               s.MembershipPlan.Name,
               s.StartDate,
               s.EndDate,
               s.Status
           )).FirstOrDefaultAsync(x => x.Id == id);

        if (response is null)
            return Result.Failure<SubscriptionResponse>(SubscriptionErrors.SubscriptionNotFound);

        return Result.Success(response);
    }

    public async Task<Result<UserSubscriptionResponse>> GetMySubscriptionAsync(string userId, CancellationToken cancellationToken)
    {
        var response = await _context.Subscriptions
            .Where(s => s.UserId == userId && s.Status == SubscriptionStatus.Active)
            .Select(s => new UserSubscriptionResponse(
                s.Trainer == null ? null : $"{s.Trainer.ApplicationUser!.FirstName} {s.Trainer.ApplicationUser.LastName}",
                s.MembershipPlan!.Name,
                s.StartDate,
                s.EndDate,
                s.Status
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return Result.Failure<UserSubscriptionResponse>(SubscriptionErrors.SubscriptionNotFound);

        return Result.Success(response);
    }
    public Task<Result> AddAsync(string userId, SubscriptionRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> CancelAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

   
}
