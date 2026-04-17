namespace Gym_Management_System.Contracts.MembershipPlan;

public record MembershipPlanResponse(int Id, string Name, string Description, decimal Price, int DurationInDays, bool IsActive);

