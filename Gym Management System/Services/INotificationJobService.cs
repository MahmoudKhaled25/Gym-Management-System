namespace GymManagementSystem.Services;

public interface INotificationJobService
{
    Task SendOfferToAllMembersAsync(string message);

}
