using GymManagementSystem.Contracts.WorkoutPlanExercise;

namespace GymManagementSystem.Services;

public interface IWorkoutPlanExerciseService
{
    Task<Result<WorkoutPlanExercisesGroupedResponse>> GetWorkoutPlanExerciseAsync(int workoutPlanId, CancellationToken cancellationToken = default);
    Task<Result> AddExerciseToPlanAsync(int workoutPlanId, WorkoutPlanExerciseRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateExerciseInPlanAsync(int workoutPlanExerciseId, UpdateWorkoutPlanExerciseRequest request, CancellationToken cancellationToken = default);
    Task<Result> RemoveExerciseFromPlanAsync(int workoutPlanExerciseId, CancellationToken cancellationToken = default);

}