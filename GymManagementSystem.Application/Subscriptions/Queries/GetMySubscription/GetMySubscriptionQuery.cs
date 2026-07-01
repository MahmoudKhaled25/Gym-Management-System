using GymManagementSystem.Application.Subscriptions.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Queries.GetMySubscription;

public record GetMySubscriptionQuery(string UserId) : IRequest<Result<UserSubscriptionDto>>;

