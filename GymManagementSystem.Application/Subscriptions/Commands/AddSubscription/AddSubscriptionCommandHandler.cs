using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Application.Subscriptions.Commands.AddSubscription;

public class AddSubscriptionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddSubscriptionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.Repository<MembershipPlan>().SingleOrDefaultAsync(x => x.Id == request.MembershipPlanId && x.IsActive, cancellationToken);
        if (plan is null)
        {
            return Result.Failure(MembershipPlanErrors.PlanNotFound);
        }
        var isMemberHasActiveSubscription = await _unitOfWork.Repository<Subscription>()
           .Query()
           .AnyAsync(x => x.UserId == request.UserId && x.Status == SubscriptionStatus.Active, cancellationToken);
        if (isMemberHasActiveSubscription)
            return Result.Failure(SubscriptionErrors.SubscriptionExists);

        var startDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var endDate = startDate.AddDays(plan.DurationInDays);

        var trainer = plan.SessionsPerMonth > 0
    ? await _unitOfWork.Repository<Trainer>()
        .Query()
        .Where(t => t.IsActive)
        .OrderBy(t => _unitOfWork.Repository<Subscription>()
            .Query()
            .Count(s => s.TrainerId == t.UserId && s.Status == SubscriptionStatus.Active))
        .FirstOrDefaultAsync(cancellationToken)
    : null;

        var subscription = new Subscription
        {
            UserId = request.UserId,
            MembershipPlanId = plan.Id,
            TrainerId = trainer?.UserId,
            StartDate = startDate,
            EndDate = endDate,
            Status = SubscriptionStatus.Active,

        };
        await _unitOfWork.Repository<Subscription>().AddAsync(subscription);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
