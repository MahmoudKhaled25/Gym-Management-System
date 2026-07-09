using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Queries.Get_By_Id;

public class GetPlanByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPlanByIdQuery, Result<MembershipPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<MembershipPlanDto>> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.Repository<MembershipPlan>().GetByIdAsync(request.Id);

        if (plan == null)
            return Result.Failure<MembershipPlanDto>(MembershipPlanErrors.PlanNotFound);

        var planDto = plan.Adapt<MembershipPlanDto>();
        return Result.Success(planDto);
    }
}
