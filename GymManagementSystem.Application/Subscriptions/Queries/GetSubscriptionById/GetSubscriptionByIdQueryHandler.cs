using GymManagementSystem.Application.Subscriptions.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Queries.GetSubscriptionById;

public class GetSubscriptionByIdQueryHandler(ISubscriptionQueries subscriptionQueries) : IRequestHandler<GetSubscriptionByIdQuery, Result<SubscriptionDto>>
{
    private readonly ISubscriptionQueries _subscriptionQueries = subscriptionQueries;

    public async Task<Result<SubscriptionDto>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
       var result = await _subscriptionQueries.GetSubscriptionByIdAsync(request.Id, cancellationToken);
        return result;
    }
}
