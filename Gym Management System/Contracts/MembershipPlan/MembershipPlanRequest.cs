namespace Gym_Management_System.Contracts.MembershipPlan;

public record MembershipPlanRequest
(string Name,string Description,decimal Price,int DurationInDays);
