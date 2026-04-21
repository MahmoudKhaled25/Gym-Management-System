using Gym_Management_System.Contracts.Subscription;
using Gym_Management_System.Persistence;

namespace Gym_Management_System.Services;

public class SubscriptionService(ApplicationDbContext context,UserManager<ApplicationUser> userManager) : ISubscriptionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<IEnumerable<SubscriptionResponse>>> GetAllAsync() {

        var response = await _context.Subscriptions
    .Select(s => new SubscriptionResponse(
        s.Id,
        s.User!.Id,
        $"{s.User.FirstName} {s.User.LastName}",
        s.Trainer == null ?  null : s.Trainer.UserId ,
        s.Trainer == null ? null : $"{s.Trainer.ApplicationUser!.FirstName} {s.Trainer.ApplicationUser.LastName}",
        s.MembershipPlan.Id,
        s.MembershipPlan.Name,
        s.StartDate,
        s.EndDate,
        s.Status
    ))
    .AsNoTracking() 
    .ToListAsync();


    return Result.Success(response.AsEnumerable());


    }
    
public async Task<Result> AddSubscription(SubscriptionRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

}
