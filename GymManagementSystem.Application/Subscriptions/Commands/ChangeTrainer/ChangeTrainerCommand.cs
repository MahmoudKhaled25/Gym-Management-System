using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;

namespace GymManagementSystem.Application.Subscriptions.Commands.ChangeTrainer;

public record ChangeTrainerCommand(string TrainerId, int SubscriptionId) : IRequest<Result>;
