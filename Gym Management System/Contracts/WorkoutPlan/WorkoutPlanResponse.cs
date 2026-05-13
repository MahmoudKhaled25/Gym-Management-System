namespace GymManagementSystem.Contracts.WorkoutPlan;

public record WorkoutPlanResponse(int Id,string Name, string Description,string? TrainerName,string MemberName);
