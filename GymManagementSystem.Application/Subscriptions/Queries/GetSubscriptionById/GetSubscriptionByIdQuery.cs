using GymManagementSystem.Application.Subscriptions.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;

namespace GymManagementSystem.Application.Subscriptions.Queries.GetSubscriptionById;

public record GetSubscriptionByIdQuery(int Id) : IRequest<Result<SubscriptionDto>>;
