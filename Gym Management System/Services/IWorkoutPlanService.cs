using GymManagementSystem.Contracts.WorkoutPlan;

namespace GymManagementSystem.Services;

public interface IWorkoutPlanService
{
    Task<Result<IEnumerable<WorkoutPlanResponse>>> GetAllAsync(string? trainerId,CancellationToken cancellationToken = default!);

    Task<Result> AddAsync(WorkoutPlanRequest request,CancellationToken cancellationToken = default!);

  
}
