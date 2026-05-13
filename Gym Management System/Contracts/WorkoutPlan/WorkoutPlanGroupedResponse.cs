namespace GymManagementSystem.Contracts.WorkoutPlan;

public record WorkoutPlanGroupedResponse(
    string? TrainerName,
    IEnumerable<WorkoutPlanResponse> Plans
);
