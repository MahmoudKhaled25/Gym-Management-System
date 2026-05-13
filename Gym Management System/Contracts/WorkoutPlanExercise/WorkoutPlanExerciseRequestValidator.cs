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
            .InclusiveBetween(1, 100).WithMessage("Sets must be between 1 and 100.");
        RuleFor(x => x.Reps)
            .NotEmpty().WithMessage("Repetitions is required.")
                 .InclusiveBetween(1, 100).WithMessage("Reps must be between 1 and 100.");
        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage("Weight is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Weight must be greater than or equal to 0.");
        RuleFor(x => x.RestTime)
            .NotEmpty().WithMessage("Rest Time is required.")
            .InclusiveBetween(0, 3600).WithMessage("Rest Time must be between 0 and 3600 seconds.");
    }
}
