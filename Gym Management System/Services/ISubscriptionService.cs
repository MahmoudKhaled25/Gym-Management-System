using Gym_Management_System.Contracts.Subscription;
namespace Gym_Management_System.Services;

public interface ISubscriptionService
{
    Task<Result<IEnumerable<SubscriptionResponse>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<SubscriptionResponse>>> GetAllActiveAsync(CancellationToken cancellationToken);
    Task<Result<SubscriptionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<UserSubscriptionResponse>> GetMySubscriptionAsync(string userId, CancellationToken cancellationToken);
    Task<Result> AddAsync(string userId, SubscriptionRequest request, CancellationToken cancellationToken);
    Task<Result> CancelAsync(int id, CancellationToken cancellationToken);
}
