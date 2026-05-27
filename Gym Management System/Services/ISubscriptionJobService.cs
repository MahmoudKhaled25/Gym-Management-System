namespace GymManagementSystem.Services;

public interface ISubscriptionJobService
{
    Task ExpireSubscriptionsAsync();
    Task NotifyExpiringSubscriptionsAsync();
}
