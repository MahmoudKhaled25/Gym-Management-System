using Gym_Management_System.Enums;
using Gym_Management_System.Persistence;
using GymManagementSystem.Abstractions;
using GymManagementSystem.Contracts.Common;
using GymManagementSystem.Contracts.SubscriptionRequest;
using GymManagementSystem.Enums;

namespace GymManagementSystem.Services;

public class SubscriptionRequestService(ApplicationDbContext context, ILogger<SubscriptionRequestService> logger) : ISubscriptionRequestService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<SubscriptionRequestService> _logger = logger;

    public async Task<Result> SendRequestAsync(string userId, int membershipPlanId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();

    }
    public Task<Result<IEnumerable<SubscriptionRequestResponse>>> GetMyRequestsAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public Task<Result<PaginatedList<SubscriptionRequestResponse>>> GetAllRequestsAsync(RequestFilters filters, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ApproveRequestAsync(int requestId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RejectRequestAsync(int requestId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    
}
