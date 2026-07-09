using GymManagementSystem.Application.SubscriptionRequests.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.SubscriptionRequests.Queries.GetMySubscriptionRequests;

public record GetMySubscriptionRequestsQuery(string UserId) : IRequest<Result<IEnumerable<SubscriptionRequestDto>>>;
