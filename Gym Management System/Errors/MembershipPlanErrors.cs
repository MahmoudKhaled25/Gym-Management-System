using Gym_Management_System.Abstractions;

namespace Gym_Management_System.Errors;

public record MembershipPlanErrors
{
    public static readonly Error PlanNotFound =
   new("Plan.PlanNotFound", "Plan Not Found", StatusCodes.Status404NotFound);

    public static readonly Error PlanExists =
   new("Plan.PlanExists", "Plan Exists", StatusCodes.Status400BadRequest);
}
