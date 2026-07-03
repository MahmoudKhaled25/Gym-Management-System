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

namespace GymManagementSystem.Application.SubscriptionRequests.Commands.ApproveSubscriptionRequest;

public class ApproveSubscriptionRequestCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ApproveSubscriptionRequestCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ApproveSubscriptionRequestCommand request, CancellationToken cancellationToken)
    {
        var subscriptionRequest = await _unitOfWork.Repository<SubscriptionRequest>()
           .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (subscriptionRequest is null)
            return Result.Failure(SubscriptionRequestErrors.RequestNotFound);

        if (subscriptionRequest.Status != SubscriptionRequestStatus.Pending)
            return Result.Failure(SubscriptionRequestErrors.AlreadyProcessed);

        var hasActiveSubscription = await _unitOfWork.Repository<Subscription>()
          .Query()
          .AnyAsync(s =>
              s.UserId == subscriptionRequest.UserId &&
              s.Status == SubscriptionStatus.Active,
              cancellationToken);

        if (hasActiveSubscription)
            return Result.Failure(SubscriptionErrors.UserAlreadyHasActiveSubscription);

        var plan = await _unitOfWork.Repository<MembershipPlan>()
            .FirstOrDefaultAsync(p => p.Id == subscriptionRequest.MembershipPlanId && p.IsActive, cancellationToken);

        if (plan is null)
            return Result.Failure(MembershipPlanErrors.PlanNotFound);

        var trainer = plan.SessionsPerMonth > 0
            ? await _unitOfWork.Repository<Trainer>()
            .   Query()
                .Where(t => t.IsActive)
                .OrderBy(t => _unitOfWork.Repository<Subscription>()
                .Query()
                .Count(s => s.TrainerId == t.UserId && s.Status == SubscriptionStatus.Active))
                .FirstOrDefaultAsync(cancellationToken)
            : null;

        var subscription = new Subscription
        {
            UserId = subscriptionRequest.UserId,
            MembershipPlanId = subscriptionRequest.MembershipPlanId,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(plan.DurationInDays),
            Status = SubscriptionStatus.Active,
            TrainerId = trainer?.UserId
        };

        subscriptionRequest.Status = SubscriptionRequestStatus.Approved;

        await _unitOfWork.Repository<Subscription>().AddAsync(subscription);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
