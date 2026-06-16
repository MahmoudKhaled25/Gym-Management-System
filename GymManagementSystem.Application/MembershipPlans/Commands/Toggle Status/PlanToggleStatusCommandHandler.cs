using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Commands.Toggle_Status;

public class PlanToggleStatusCommandHandler(IUnitOfWork unitOfWork,IMemoryCache memoryCache) : IRequestHandler<PlanToggleStatusCommand, Result>
{
    private const string _allPlansCacheKey = "MembershipPlans_All";
    private const string _activePlansCacheKey = "MembershipPlans_Active";
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<Result> Handle(PlanToggleStatusCommand request, CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.Repository<MembershipPlan>().GetByIdAsync(request.Id);
        if(plan == null) 
            return Result.Failure(MembershipPlanErrors.PlanNotFound);

        plan.IsActive = !plan.IsActive;
         _unitOfWork.Repository<MembershipPlan>().Update(plan);
       await _unitOfWork.SaveChangesAsync(cancellationToken);
        _memoryCache.Remove(_allPlansCacheKey);
        _memoryCache.Remove(_activePlansCacheKey);
        return Result.Success();

    }
}
