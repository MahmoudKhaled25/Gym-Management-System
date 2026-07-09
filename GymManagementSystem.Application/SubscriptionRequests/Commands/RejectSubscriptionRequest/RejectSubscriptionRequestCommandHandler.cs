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

namespace GymManagementSystem.Application.SubscriptionRequests.Commands.RejectSubscriptionRequest;

public class RejectSubscriptionRequestCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RejectSubscriptionRequestCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RejectSubscriptionRequestCommand request, CancellationToken cancellationToken)
    {
        var subscriptionRequest = await _unitOfWork.Repository<SubscriptionRequest>()
            .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (subscriptionRequest is null)
            return Result.Failure(SubscriptionRequestErrors.RequestNotFound);

        if (subscriptionRequest.Status != SubscriptionRequestStatus.Pending)
            return Result.Failure(SubscriptionRequestErrors.AlreadyProcessed);

        subscriptionRequest.Status = SubscriptionRequestStatus.Rejected;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
