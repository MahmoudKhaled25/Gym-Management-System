using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Subscriptions.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Queries;

public interface ISubscriptionQueries
{
    Task<Result<PaginatedList<SubscriptionDto>>> GetAllSubscriptionsAsync(RequestFilters filters, CancellationToken cancellationToken);

    Task<Result<PaginatedList<SubscriptionDto>>> GetActiveSubscriptionsAsync(RequestFilters filters, CancellationToken cancellationToken);

    Task<Result<UserSubscriptionDto>> GetMySubscriptionAsync(string userId, CancellationToken cancellationToken);

    Task<Result<SubscriptionDto>> GetSubscriptionByIdAsync(int id, CancellationToken cancellationToken);

}
