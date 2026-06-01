using Gym_Management_System.Enums;
using Gym_Management_System.Persistence;

namespace GymManagementSystem.Services;

public class SubscriptionJobService(ApplicationDbContext context,INotificationService notificationService,ILogger<SubscriptionJobService> logger) : ISubscriptionJobService
{
    private readonly ApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;
    private readonly ILogger<SubscriptionJobService> _logger = logger;

    public async Task ExpireSubscriptionsAsync()
    {
        var expiredSubscriptions = await _context.Subscriptions
             .Where(s => s.Status == SubscriptionStatus.Active
                      && s.EndDate < DateOnly.FromDateTime(DateTime.UtcNow))
             .ToListAsync();

        foreach(var expiredSubscription in expiredSubscriptions) 
            expiredSubscription.Status = SubscriptionStatus.Expired;

        _logger.LogInformation("Expired {Count} subscriptions", expiredSubscriptions.Count);

        await _context.SaveChangesAsync();
    }
    public async Task NotifyExpiringSubscriptionsAsync()
    {
        var threeDaysLater = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3));

        var expiringSubscriptions = await _context.Subscriptions
            .Include(s => s.User)
            .Where(s => s.Status == SubscriptionStatus.Active
                     && s.EndDate == threeDaysLater
                     && s.User!.PhoneNumber != null)
            .ToListAsync();

        foreach (var subscription in expiringSubscriptions)
        {
            var message = $"Dear {subscription.User!.FirstName}, " +
                $"your gym subscription will expire on {subscription.EndDate:MMMM dd, yyyy}. Please renew it to continue enjoying our services.";

            await _notificationService.SendWhatsAppAsync(subscription.User.PhoneNumber!,message);
        }
        _logger.LogInformation("Sent {Count} expiry notifications", expiringSubscriptions.Count);

    }
}

