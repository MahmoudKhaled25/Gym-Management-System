using GymManagementSystem.Application.Notifications.Interfaces;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Application.Interfaces;


namespace GymManagementSystem.Infrastructure.Services;

public class NotificationJobService(INotificationService notificationService, ILogger<NotificationJobService> logger, ApplicationDbContext context) : INotificationJobService
{
    private readonly INotificationService _notificationService = notificationService;
    private readonly ILogger<NotificationJobService> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public async Task SendOfferToAllMembersAsync(string message)
    {
        var numbers = await _context.Users
                 .Where(u => u.PhoneNumber != null)
                 .Select(u => u.PhoneNumber!)
                 .Distinct()
                 .ToListAsync();

        foreach (var number in numbers)
        {
            try
            {
                await _notificationService.SendWhatsAppAsync(number, message);
                _logger.LogInformation("Sent offer notification to {PhoneNumber}", number);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send offer notification to {PhoneNumber}", number);
            }
        }
    }
}
