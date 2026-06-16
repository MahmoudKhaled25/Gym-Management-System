using GymManagementSystem.Domain.Repositories;
using MediatR;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using Mapster;
using Microsoft.Extensions.Caching.Memory;

namespace GymManagementSystem.Application.MembershipPlans.Queries.Get_All_Plans;

public class GetAllPlansQueryHandler(IUnitOfWork unitOfWork,IMemoryCache memoryCache) : IRequestHandler<GetAllPlansQuery, Result<IEnumerable<MembershipPlanDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _allPlansCacheKey = "MembershipPlans_All";
    public async Task<Result<IEnumerable<MembershipPlanDto>>> Handle(GetAllPlansQuery request, CancellationToken cancellationToken)
    {

        if (_memoryCache.TryGetValue(_allPlansCacheKey, out IEnumerable<MembershipPlanDto>? cached))
            return Result.Success(cached!);

        var plans = await _unitOfWork.Repository<MembershipPlan>().GetAllAsync();
        _memoryCache.Set(_allPlansCacheKey, plans, TimeSpan.FromMinutes(30));

        return Result.Success(plans.Adapt<IEnumerable<MembershipPlanDto>>());

    }
}
