using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Application.Trainers.Queries.GetTrainerMembers;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Queries.GetMyMembers;

public class GetMyMembersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMyMembersQuery, Result<IEnumerable<TrainerMembersDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<IEnumerable<TrainerMembersDto>>> Handle(GetMyMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _unitOfWork.Repository<Subscription>()
                          .Query()
                          .Where(x => x.TrainerId == request.TrainerId && x.Status == SubscriptionStatus.Active)
                           .Select(x => new TrainerMembersDto(
                               x.UserId,
                               x.User!.FirstName + " " + x.User.LastName,
                               x.User.PhoneNumber,
                               x.MembershipPlan!.Name,
                               x.EndDate
                               ))
                           .AsNoTracking()
                           .ToListAsync(cancellationToken);
        return Result.Success(members.AsEnumerable());
    }
}
