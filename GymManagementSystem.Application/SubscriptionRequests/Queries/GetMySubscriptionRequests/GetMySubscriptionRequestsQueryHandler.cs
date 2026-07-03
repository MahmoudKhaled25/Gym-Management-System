using GymManagementSystem.Application.SubscriptionRequests.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.SubscriptionRequests.Queries.GetMySubscriptionRequests;

public class GetMySubscriptionRequestsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMySubscriptionRequestsQuery, Result<IEnumerable<SubscriptionRequestDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<SubscriptionRequestDto>>> Handle(GetMySubscriptionRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _unitOfWork.Repository<SubscriptionRequest>()
            .Query()
            .Where(r => r.UserId == request.UserId)
           .Select(x => new SubscriptionRequestDto(
               x.Id,
               x.User!.FirstName + " " + x.User.LastName,
               x.MembershipPlan!.Name,
               x.MembershipPlan.Price,
               x.Status,
               x.RequestedAt
           ))
           .ToListAsync(cancellationToken);

        if (requests == null || !requests.Any())
            return Result.Failure<IEnumerable<SubscriptionRequestDto>>(SubscriptionRequestErrors.NoRequestsFound);

        return Result.Success(requests.AsEnumerable());
    }
}
