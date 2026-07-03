using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.SubscriptionRequests.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.SubscriptionRequests.Queries;

public interface ISubscriptionRequestQueries
{
    Task<Result<PaginatedList<SubscriptionRequestDto>>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken);
}
