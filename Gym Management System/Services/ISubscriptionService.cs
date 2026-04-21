using Gym_Management_System.Contracts.Subscription;
namespace Gym_Management_System.Services;

public interface ISubscriptionService
{
    Task<Result<IEnumerable<SubscriptionResponse>>> GetAllAsync();

    //Task<Result<SubscriptionResponse>> GetByIdAsync();

    Task<Result> AddSubscription(SubscriptionRequest request,CancellationToken cancellationToken);
}
