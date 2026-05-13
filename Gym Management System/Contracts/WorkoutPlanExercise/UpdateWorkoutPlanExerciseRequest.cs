namespace GymManagementSystem.Contracts.WorkoutPlanExercise;

public record UpdateWorkoutPlanExerciseRequest(
    int Sets,
    int Reps,
    float Weight,
    float RestTime
);