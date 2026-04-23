namespace Gym_Management_System.Errors;

public record ExerciseErrors
{
    public static readonly Error ExerciseNotFound =
  new("Exercise.ExerciseNotFound", "Exercise Not Found", StatusCodes.Status404NotFound);

    public static readonly Error ExerciseExists =
  new("Exercise.ExerciseExists", "Exercise Exists", StatusCodes.Status400BadRequest);
}
