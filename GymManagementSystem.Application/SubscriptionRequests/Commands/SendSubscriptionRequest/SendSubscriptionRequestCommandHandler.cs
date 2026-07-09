using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.SubscriptionRequests.Commands.SendSubscriptionRequest;

public class SendSubscriptionRequestCommandHandler(IUnitOfWork unitOfWork,ILogger<SendSubscriptionRequestCommandHandler> logger) : IRequestHandler<SendSubscriptionRequestCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<SendSubscriptionRequestCommandHandler> _logger = logger;

    public async Task<Result> Handle(SendSubscriptionRequestCommand request, CancellationToken cancellationToken)
    {
        var hasActiveSubscription = await _unitOfWork.Repository<Subscription>()
           .Query()
         .AnyAsync(s => s.UserId == request.UserId && s.Status == SubscriptionStatus.Active, cancellationToken);
        if (hasActiveSubscription)
            return Result.Failure(SubscriptionRequestErrors.UserHasActiveSubscription);

        var hasPendingRequest = await _unitOfWork.Repository<SubscriptionRequest>()
            .Query()
            .AnyAsync(r => r.UserId == request.UserId && r.Status == SubscriptionRequestStatus.Pending, cancellationToken);
        if (hasPendingRequest)
            return Result.Failure(SubscriptionRequestErrors.PendingRequestExists);

        var plan = await _unitOfWork.Repository<MembershipPlan>()
            .Query()
            .AnyAsync(p => p.Id == request.MembershipPlanId && p.IsActive, cancellationToken);
        if (!plan)
            return Result.Failure(MembershipPlanErrors.PlanNotFound);

        var subscriptionRequest = new SubscriptionRequest
        {
            UserId = request.UserId,
            MembershipPlanId = request.MembershipPlanId,
            Status = SubscriptionRequestStatus.Pending,
            RequestedAt = DateTime.UtcNow,
        };
        await _unitOfWork.Repository<SubscriptionRequest>().AddAsync(subscriptionRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User {UserId} sent a subscription request for plan {PlanId}", request.UserId, request.MembershipPlanId);
        return Result.Success();
    }
}
