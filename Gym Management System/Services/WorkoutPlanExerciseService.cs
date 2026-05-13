using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using GymManagementSystem.Contracts.WorkoutPlanExercise;
using GymManagementSystem.Errors;

namespace GymManagementSystem.Services;

public class WorkoutPlanExerciseService(ApplicationDbContext context) : IWorkoutPlanExerciseService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<Result<WorkoutPlanExercisesGroupedResponse>> GetWorkoutPlanExerciseAsync(int workoutPlanId, CancellationToken cancellationToken = default)
    {
     var exercises = await _context.WorkoutPlanExercises
    .Where(x => x.WorkoutPlanId == workoutPlanId)
    .Select(x => new
    {
        WorkoutPlanName = x.WorkoutPlan!.Name,
        Exercise = new WorkoutPlanExerciseResponse(
            x.Id,
            x.Sets,
            x.Reps,
            x.Weight,
            x.RestTime,
            x.Exercise!.Name
        )
    })
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var grouped = new WorkoutPlanExercisesGroupedResponse(
            exercises.First().WorkoutPlanName,
            exercises.Select(x => x.Exercise)
        );

        return Result.Success(grouped);
    }
    public async Task<Result> AddExerciseToPlanAsync(int workoutPlanId, WorkoutPlanExerciseRequest request, CancellationToken cancellationToken = default)
    {
        var IsWorkoutPlanExist = await _context.WorkoutPlans.AnyAsync(x => x.Id == workoutPlanId, cancellationToken);
        if (!IsWorkoutPlanExist)
        {
            return Result.Failure(WorkoutPlanErrors.WorkoutPlanNotFound);
        }
        var IsExerciseExist = await _context.Exercises.AnyAsync(x => x.Id == request.ExerciseId, cancellationToken);
        if (!IsExerciseExist)
        {
            return Result.Failure(ExerciseErrors.ExerciseNotFound);
        }
        var workoutPlanExercise = request.Adapt<WorkoutPlanExercise>();
        workoutPlanExercise.WorkoutPlanId = workoutPlanId;

        await _context.WorkoutPlanExercises.AddAsync(workoutPlanExercise, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateExerciseInPlanAsync(int workoutPlanExerciseId, UpdateWorkoutPlanExerciseRequest request, CancellationToken cancellationToken = default)
    {
        var workoutPlanExercise = await _context.WorkoutPlanExercises
            .FirstOrDefaultAsync(x => x.Id == workoutPlanExerciseId, cancellationToken);

        if (workoutPlanExercise is null)
            return Result.Failure(WorkoutPlanExerciseErrors.WorkoutPlanExerciseNotFound);

        workoutPlanExercise.Sets = request.Sets;
        workoutPlanExercise.Reps = request.Reps;
        workoutPlanExercise.Weight = request.Weight;
        workoutPlanExercise.RestTime = request.RestTime;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> RemoveExerciseFromPlanAsync(int workoutPlanExerciseId, CancellationToken cancellationToken = default)
    {
        var workoutPlanExercise = await _context.WorkoutPlanExercises
            .FirstOrDefaultAsync(x => x.Id == workoutPlanExerciseId, cancellationToken);

        if (workoutPlanExercise is null)
            return Result.Failure(WorkoutPlanExerciseErrors.WorkoutPlanExerciseNotFound);

        _context.WorkoutPlanExercises.Remove(workoutPlanExercise);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
