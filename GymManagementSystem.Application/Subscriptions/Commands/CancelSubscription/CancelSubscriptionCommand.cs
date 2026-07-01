using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Commands.CancelSubscription;

public record CancelSubscriptionCommand(int Id) : IRequest<Result>;

