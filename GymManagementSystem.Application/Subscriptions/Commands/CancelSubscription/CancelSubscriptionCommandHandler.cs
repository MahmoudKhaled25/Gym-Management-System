using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;

namespace GymManagementSystem.Application.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CancelSubscriptionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _unitOfWork.Repository<Subscription>().SingleOrDefaultAsync(s => s.Id == request.Id,cancellationToken);
        if (subscription is null)
            return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

        subscription.Status = SubscriptionStatus.Cancelled;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }
}
