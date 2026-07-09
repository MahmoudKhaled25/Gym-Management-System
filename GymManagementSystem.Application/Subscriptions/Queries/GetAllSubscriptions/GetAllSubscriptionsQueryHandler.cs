using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Subscriptions.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Queries.GetAllSubscriptions;

public class GetAllSubscriptionsQueryHandler(ISubscriptionQueries subscriptionQueries) : IRequestHandler<GetAllSubscriptionsQuery, Result<PaginatedList<SubscriptionDto>>>
{
    private readonly ISubscriptionQueries _subscriptionQueries = subscriptionQueries;

    public async Task<Result<PaginatedList<SubscriptionDto>>> Handle(GetAllSubscriptionsQuery request, CancellationToken cancellationToken)
    {
       var result = await _subscriptionQueries.GetAllSubscriptionsAsync(request.Filters, cancellationToken);
        return result;
    }
}
