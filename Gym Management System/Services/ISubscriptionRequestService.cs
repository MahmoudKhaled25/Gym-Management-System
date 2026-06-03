using GymManagementSystem.Abstractions;
using GymManagementSystem.Contracts.Common;
using GymManagementSystem.Contracts.SubscriptionRequest;

namespace GymManagementSystem.Services;

public interface ISubscriptionRequestService
{
    // Member
    Task<Result> SendRequestAsync(string userId, int membershipPlanId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SubscriptionRequestResponse>>> GetMyRequestsAsync(string userId, CancellationToken cancellationToken = default);

    // Admin
    Task<Result<PaginatedList<SubscriptionRequestResponse>>> GetAllRequestsAsync(RequestFilters filters, CancellationToken cancellationToken = default);
    Task<Result> ApproveRequestAsync(int requestId, CancellationToken cancellationToken = default);
    Task<Result> RejectRequestAsync(int requestId, CancellationToken cancellationToken = default);
}
