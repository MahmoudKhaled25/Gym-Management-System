namespace GymManagementSystem.Errors;

public record WorkoutPlanErrors
{
    public static readonly Error WorkoutPlanNotFound =
        new("WorkoutPlan.WorkoutPlanNotFound", "Workout Plan Not Found", StatusCodes.Status404NotFound);
     public static readonly Error WorkoutPlanExists =
        new("WorkoutPlan.WorkoutPlanExists", "Workout Plan Exists", StatusCodes.Status400BadRequest);
    public static readonly Error MemberOnly =
      new("WorkoutPlan.MemberOnly", "Member Only", StatusCodes.Status400BadRequest);
}
