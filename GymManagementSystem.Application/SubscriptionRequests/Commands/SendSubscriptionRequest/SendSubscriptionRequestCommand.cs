using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;

namespace GymManagementSystem.Application.SubscriptionRequests.Commands.SendSubscriptionRequest;

public record SendSubscriptionRequestCommand(int MembershipPlanId,string UserId) : IRequest<Result>;
