using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Commands.ChangeTrainer;

public class ChangeTrainerCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeTrainerCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ChangeTrainerCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _unitOfWork.Repository<Subscription>()
            .SingleOrDefaultAsync(s => s.Id == request.SubscriptionId, cancellationToken);

        if (subscription is null || subscription.Status != SubscriptionStatus.Active)
            return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

        var plan = await _unitOfWork.Repository<MembershipPlan>()
        .SingleOrDefaultAsync(p => p.Id == subscription.MembershipPlanId, cancellationToken);

        if (plan is null || plan.SessionsPerMonth == 0)
            return Result.Failure(SubscriptionErrors.PlanHasNoTrainer);

        var trainer = await _unitOfWork.Repository<Trainer>()
            .SingleOrDefaultAsync(t => t.UserId == request.TrainerId && t.IsActive, cancellationToken);
        if (trainer is null)
            return Result.Failure(TrainerErrors.TrainerNotFound);

        subscription.TrainerId = request.TrainerId;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
