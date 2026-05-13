namespace GymManagementSystem.Contracts.WorkoutPlanExercise;

public record WorkoutPlanExerciseResponse(
    int Id,
    int Sets,
    int Reps,
    float Weight,
    float RestTime,
    string? ExerciseName
);

public record WorkoutPlanExercisesGroupedResponse(
    string WorkoutPlanName,
    IEnumerable<WorkoutPlanExerciseResponse> Exercises
);

