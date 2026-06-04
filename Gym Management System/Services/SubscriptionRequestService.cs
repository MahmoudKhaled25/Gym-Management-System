using Gym_Management_System.Contracts.Subscription;
using Gym_Management_System.Enums;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using GymManagementSystem.Abstractions;
using GymManagementSystem.Contracts.Common;
using GymManagementSystem.Contracts.SubscriptionRequest;
using GymManagementSystem.Entities;
using GymManagementSystem.Enums;
using GymManagementSystem.Errors;

namespace GymManagementSystem.Services;

public class SubscriptionRequestService(ApplicationDbContext context, ILogger<SubscriptionRequestService> logger) : ISubscriptionRequestService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<SubscriptionRequestService> _logger = logger;

    public async Task<Result> SendRequestAsync(string userId, int membershipPlanId, CancellationToken cancellationToken = default)
    {
        var hasActiveSubscription = await _context.Subscriptions
            .AnyAsync(s => s.UserId == userId && s.Status == SubscriptionStatus.Active, cancellationToken);
        if (hasActiveSubscription)
            return Result.Failure(SubscriptionRequestErrors.UserHasActiveSubscription);

        var hasPendingRequest = await _context.SubscriptionRequests
            .AnyAsync(r => r.UserId == userId && r.Status == SubscriptionRequestStatus.Pending, cancellationToken);
        if (hasPendingRequest)
            return Result.Failure(SubscriptionRequestErrors.PendingRequestExists);

        var plan = await _context.MembershipPlans
            .AnyAsync(p => p.Id == membershipPlanId && p.IsActive, cancellationToken);
        if (!plan)
            return Result.Failure(MembershipPlanErrors.PlanNotFound);

        var request = new SubscriptionRequest
        {
            UserId = userId,
            MembershipPlanId = membershipPlanId,
            Status = SubscriptionRequestStatus.Pending,
            RequestedAt = DateTime.UtcNow,
        };
        _context.SubscriptionRequests.Add(request);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User {UserId} sent a subscription request for plan {PlanId}", userId, membershipPlanId);
        return Result.Success();
    }
    public Task<Result<IEnumerable<SubscriptionRequestResponse>>> GetMyRequestsAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public async Task<Result<PaginatedList<SubscriptionRequestResponse>>> GetAllRequestsAsync(
     RequestFilters filters,
     CancellationToken cancellationToken = default)
    {
        var query = _context.SubscriptionRequests
            .Where(r => string.IsNullOrEmpty(filters.SearchValue) ||
                        (r.User!.FirstName + " " + r.User.LastName).Contains(filters.SearchValue) ||
                        r.MembershipPlan!.Name.Contains(filters.SearchValue));

        var sortedQuery = filters.SortColumn?.ToLower() switch
        {
            "membername" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.User!.FirstName)
                : query.OrderBy(r => r.User!.FirstName),

            "status" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.Status)
                : query.OrderBy(r => r.Status),

            "requestedat" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.RequestedAt)
                : query.OrderBy(r => r.RequestedAt),

            "price" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.MembershipPlan!.Price)
                : query.OrderBy(r => r.MembershipPlan!.Price),

            "membershipplanname" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.MembershipPlan!.Name)
                : query.OrderBy(r => r.MembershipPlan!.Name),

            _ => query.OrderBy(r => r.RequestedAt)
        };

        var finalQuery = sortedQuery.Select(x => new SubscriptionRequestResponse(
            x.Id,
            x.User!.FirstName + " " + x.User.LastName,
            x.MembershipPlan!.Name,
            x.MembershipPlan.Price,
            x.Status,
            x.RequestedAt
        ));

        var result = await PaginatedList<SubscriptionRequestResponse>.CreateAsync(
            finalQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

        return Result.Success(result);
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
