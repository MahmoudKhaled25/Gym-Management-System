namespace GymManagementSystem.Errors;

public record WorkoutPlanExerciseErrors
{
    public static readonly Error WorkoutPlanExerciseNotFound =
        new("WorkoutPlanExercise.WorkoutPlanExerciseNotFound", "Workout Plan Exercise Not Found", StatusCodes.Status404NotFound);
     public static readonly Error WorkoutPlanExerciseExists =
        new("WorkoutPlanExercise.WorkoutPlanExerciseExists", "Workout Plan Exercise Exists", StatusCodes.Status400BadRequest);
}
