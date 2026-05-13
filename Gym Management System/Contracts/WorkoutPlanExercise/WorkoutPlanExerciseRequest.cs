namespace GymManagementSystem.Contracts.WorkoutPlanExercise;

public record WorkoutPlanExerciseRequest(
    int ExerciseId,
    int Sets,
    int Reps,
    float Weight,
    float RestTime
);
