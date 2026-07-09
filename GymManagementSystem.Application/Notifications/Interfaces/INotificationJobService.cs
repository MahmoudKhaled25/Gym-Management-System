namespace GymManagementSystem.Application.Notifications.Interfaces;

public interface INotificationJobService
{
    Task SendOfferToAllMembersAsync(string message);

}
