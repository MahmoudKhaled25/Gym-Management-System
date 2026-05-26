namespace GymManagementSystem.Services;

public interface INotificationService
{
    Task SendWhatsAppAsync(string to, string message);
}
