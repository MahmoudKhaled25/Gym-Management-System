using GymManagementSystem.Abstractions;
using GymManagementSystem.Contracts.Common;
using GymManagementSystem.Contracts.WorkoutPlan;

namespace GymManagementSystem.Services;

public interface IWorkoutPlanService
{
    Task<Result<PaginatedList<WorkoutPlanResponse>>> GetAllAsync(RequestFilters filters, string? trainerId, CancellationToken cancellationToken = default!);

    Task<Result> AddAsync(WorkoutPlanRequest request,CancellationToken cancellationToken = default!);

    Task<Result<IEnumerable<WorkoutPlanGroupedResponse>>> GetMemberWorkoutPlanAsync(string memberId, CancellationToken cancellationToken = default!);

    Task<Result> UpdateAsync(int id, WorkoutPlanRequest request, CancellationToken cancellationToken = default!);

    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default!);

}
