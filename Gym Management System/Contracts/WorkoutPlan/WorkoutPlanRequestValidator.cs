namespace GymManagementSystem.Contracts.WorkoutPlan;

public class WorkoutPlanRequestValidator : AbstractValidator<WorkoutPlanRequest>
{
    public WorkoutPlanRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Workout plan name is required.")
            .Length(3,100).WithMessage("Workout plan name must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Workout plan description is required.")
            .Length(3,500).WithMessage("Workout plan description must be between 3 and 500 characters.");

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member ID is required.");


    }
}
