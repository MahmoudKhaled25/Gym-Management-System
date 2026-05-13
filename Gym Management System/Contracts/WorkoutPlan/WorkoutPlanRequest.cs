namespace GymManagementSystem.Contracts.WorkoutPlan;

public record WorkoutPlanRequest(
    string Name,
    string Description,
    string MemberId,
    string? TrainerId
);
