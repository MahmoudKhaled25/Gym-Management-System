using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Subscriptions.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Queries.GetActiveSubscriptions;

public class GetActiveSubscriptionsQueryHandler(ISubscriptionQueries subscriptionQueries) : IRequestHandler<GetActiveSubscriptionsQuery, Result<PaginatedList<SubscriptionDto>>>
{
    private readonly ISubscriptionQueries _subscriptionQueries = subscriptionQueries;

    public async Task<Result<PaginatedList<SubscriptionDto>>> Handle(GetActiveSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var result = await _subscriptionQueries.GetActiveSubscriptionsAsync(request.Filters, cancellationToken);
        return result;
    }
}
