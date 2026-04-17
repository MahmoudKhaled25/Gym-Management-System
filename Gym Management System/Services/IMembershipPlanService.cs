using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.MembershipPlan;

namespace Gym_Management_System.Services;

public interface IMembershipPlanService
{
    Task<Result<IEnumerable<MembershipPlanResponse>>> GetAllAsync(CancellationToken cancellationToken);

    Task<Result<IEnumerable<MembershipPlanResponse>>> GetAllActiveAsync();

    Task<Result<MembershipPlanResponse>> GetByIdAsync(int id,CancellationToken cancellationToken);

    Task<Result<MembershipPlanResponse>> AddAsync(MembershipPlanRequest request, CancellationToken cancellationToken);

    Task<Result> UpdateAsync(int id,MembershipPlanRequest request, CancellationToken cancellationToken);

    Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken);


}
