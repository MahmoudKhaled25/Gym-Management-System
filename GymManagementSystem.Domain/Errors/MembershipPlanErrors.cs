using GymManagementSystem.Domain.Abstractions.Error;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.Domain.Errors;

public record MembershipPlanErrors
{
    public static readonly Error PlanNotFound =
   new("Plan.PlanNotFound", "Plan Not Found", StatusCodes.Status404NotFound);

    public static readonly Error PlanExists =
   new("Plan.PlanExists", "Plan Exists", StatusCodes.Status400BadRequest);
}
