using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Queries.Get_Active_Plans;

public class GetActivePlansQueryHandler(IUnitOfWork unitOfWork,IMemoryCache memoryCache) : IRequestHandler<GetActivePlansQuery, Result<IEnumerable<MembershipPlanDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _activePlansCacheKey = "MembershipPlans_Active";


    public async Task<Result<IEnumerable<MembershipPlanDto>>> Handle(GetActivePlansQuery request, CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(_activePlansCacheKey, out IEnumerable<MembershipPlanDto>? cached))
            return Result.Success(cached!);

        var plans = await _unitOfWork.Repository<MembershipPlan>()
             .Query()
             .Where(x => x.IsActive)
             .ProjectToType<MembershipPlanDto>()
             .ToListAsync(cancellationToken);

        _memoryCache.Set(_activePlansCacheKey, plans,TimeSpan.FromMinutes(30));

        return Result.Success(plans.AsEnumerable());

    }
}
