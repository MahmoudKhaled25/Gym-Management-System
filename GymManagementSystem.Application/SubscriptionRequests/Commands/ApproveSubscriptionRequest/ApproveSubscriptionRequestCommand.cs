using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.SubscriptionRequests.Commands.ApproveSubscriptionRequest;

public record ApproveSubscriptionRequestCommand(int RequestId) : IRequest<Result>;
