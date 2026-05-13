using GymManagementSystem.Contracts.WorkoutPlan;

namespace GymManagementSystem.Services;

public interface IWorkoutPlanService
{
    Task<Result<IEnumerable<WorkoutPlanResponse>>> GetAllAsync(string? trainerId,CancellationToken cancellationToken = default!);

  
}
