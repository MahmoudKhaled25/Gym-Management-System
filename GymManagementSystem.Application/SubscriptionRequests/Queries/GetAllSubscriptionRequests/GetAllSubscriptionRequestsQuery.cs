using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.SubscriptionRequests.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.SubscriptionRequests.Queries.GetAllSubscriptionRequests;

public record GetAllSubscriptionRequestsQuery(RequestFilters Filters) : IRequest<Result<PaginatedList<SubscriptionRequestDto>>>;
