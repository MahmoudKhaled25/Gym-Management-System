using GymManagementSystem.Application.Subscriptions.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Queries.GetMySubscription;

public class GetMySubscriptionQueryHandler(ISubscriptionQueries subscriptionQueries) : IRequestHandler<GetMySubscriptionQuery, Result<UserSubscriptionDto>>
{
    private readonly ISubscriptionQueries _subscriptionQueries = subscriptionQueries;

    public async Task<Result<UserSubscriptionDto>> Handle(GetMySubscriptionQuery request, CancellationToken cancellationToken)
    {
        var result = await _subscriptionQueries.GetMySubscriptionAsync(request.UserId, cancellationToken);
        return result;
    }
}
