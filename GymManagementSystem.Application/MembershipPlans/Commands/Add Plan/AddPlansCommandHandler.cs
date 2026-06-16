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

namespace GymManagementSystem.Application.MembershipPlans.Commands.Add_Plan;

public class AddPlansCommandHandler(IUnitOfWork unitOfWork,IMemoryCache memoryCache): IRequestHandler<AddPlansCommand, Result<MembershipPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMemoryCache _memoryCache = memoryCache;

    private const string _allPlansCacheKey = "MembershipPlans_All";
    private const string _activePlansCacheKey = "MembershipPlans_Active";

    public async Task<Result<MembershipPlanDto>> Handle(AddPlansCommand request,CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim().ToLower();

        var isPlanExists = await _unitOfWork.Repository<MembershipPlan>()
            .Query()
            .AnyAsync(
                x => x.Name.ToLower() == normalizedName,
                cancellationToken);

        if (isPlanExists)
            return Result.Failure<MembershipPlanDto>(MembershipPlanErrors.PlanExists);

        var plan = request.Adapt<MembershipPlan>();

        plan.Name = request.Name.Trim();

        await _unitOfWork.Repository<MembershipPlan>()
            .AddAsync(plan);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _memoryCache.Remove(_allPlansCacheKey);
        _memoryCache.Remove(_activePlansCacheKey);

        var response = plan.Adapt<MembershipPlanDto>();

        return Result.Success(response);
    }
}
