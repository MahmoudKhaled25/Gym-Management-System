namespace GymManagementSystem.Application.Interfaces;

public interface ISubscriptionJobService
{
    Task ExpireSubscriptionsAsync();
    Task NotifyExpiringSubscriptionsAsync();
}
