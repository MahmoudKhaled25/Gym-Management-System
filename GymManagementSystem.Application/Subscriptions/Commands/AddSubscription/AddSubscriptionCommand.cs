using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Commands.AddSubscription;

public record AddSubscriptionCommand(string UserId, int MembershipPlanId) : IRequest<Result>;
