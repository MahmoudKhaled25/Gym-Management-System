using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.SubscriptionRequests.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;

namespace GymManagementSystem.Application.SubscriptionRequests.Queries.GetAllSubscriptionRequests;

public class GetAllSubscriptionRequestsQueryHandler(ISubscriptionRequestQueries subscriptionRequestQueries) : IRequestHandler<GetAllSubscriptionRequestsQuery, Result<PaginatedList<SubscriptionRequestDto>>>
{
    private readonly ISubscriptionRequestQueries _subscriptionRequestQueries = subscriptionRequestQueries;

    public async Task<Result<PaginatedList<SubscriptionRequestDto>>> Handle(GetAllSubscriptionRequestsQuery request, CancellationToken cancellationToken)
    {
        var result = await _subscriptionRequestQueries.GetAllAsync(request.Filters, cancellationToken);
        return result;
    }
}
