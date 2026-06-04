using Gym_Management_System.Enums;
using Gym_Management_System.Persistence;

namespace GymManagementSystem.Services;

public class NotificationJobService(INotificationService notificationService, ILogger<NotificationJobService> logger,ApplicationDbContext context) : INotificationJobService
{
    private readonly INotificationService _notificationService = notificationService;
    private readonly ILogger<NotificationJobService> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public async Task SendOfferToAllMembersAsync(string message)
    {
        var members = await _context.Subscriptions
            .Where(s => s.Status == SubscriptionStatus.Active
                     && s.User!.PhoneNumber != null)
            .Select(s => s.User!.PhoneNumber!)
            .Distinct()
            .ToListAsync();

        foreach (var member in members)
        {
            try
            {
                await _notificationService.SendWhatsAppAsync(member, message);
                _logger.LogInformation("Sent offer notification to {PhoneNumber}", member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send offer notification to {PhoneNumber}", member);
            }
        }
    }
}
