namespace GymManagementSystem.Contracts.WorkoutPlanExercise;

public class WorkoutPlanExerciseRequestValidator : AbstractValidator<WorkoutPlanExerciseRequest>
{
    public WorkoutPlanExerciseRequestValidator()
    {
        
        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage("Exercise Id is required.")
            .GreaterThan(0).WithMessage("Exercise Id must be greater than 0.");
        RuleFor(x => x.Sets)
            .NotEmpty().WithMessage("Sets is required.")
            .GreaterThan(0).WithMessage("Sets must be greater than 0.");
        RuleFor(x => x.Reps)
            .NotEmpty().WithMessage("Repetitions is required.")
            .GreaterThan(0).WithMessage("Repetitions must be greater than 0.");
        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage("Weight is required.")
            .GreaterThan(0).WithMessage("Weight must be greater than 0.");
        RuleFor(x => x.RestTime)
            .NotEmpty().WithMessage("Rest Time is required.")
            .GreaterThan(0).WithMessage("Rest Time must be greater than 0.");
    }
}
