using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Subscriptions.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Queries.GetAllSubscriptions;

public record GetAllSubscriptionsQuery(RequestFilters Filters) : IRequest<Result<PaginatedList<SubscriptionDto>>>;

