using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Commands.Update_Plan;

public class UpdatePlanCommandHandler(IUnitOfWork unitOfWork,IMemoryCache memoryCache) : IRequestHandler<UpdatePlanCommand, Result<MembershipPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _allPlansCacheKey = "MembershipPlans_All";
    private const string _activePlansCacheKey = "MembershipPlans_Active";

    public async Task<Result<MembershipPlanDto>> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.Repository<MembershipPlan>().GetByIdAsync(request.Id);

        if (plan is null)
            return Result.Failure<MembershipPlanDto>(MembershipPlanErrors.PlanNotFound);

        var normalizedName = request.Name.Trim().ToLower();

        var isNameExists = await _unitOfWork.Repository<MembershipPlan>().Query()
            .AnyAsync(x => x.Id != request.Id && x.Name.ToLower() == normalizedName, cancellationToken);

        if (isNameExists)
            return Result.Failure<MembershipPlanDto>(MembershipPlanErrors.PlanExists);

        plan.Name = request.Name.Trim();
        plan.Description = request.Description;
        plan.Price = request.Price;
        plan.DurationInDays = request.DurationInDays;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _memoryCache.Remove(_allPlansCacheKey);
        _memoryCache.Remove(_activePlansCacheKey);
        return Result.Success(plan.Adapt<MembershipPlanDto>());
    }
}
