namespace GymManagementSystem.Contracts.WorkoutPlanExercise;

public class UpdateWorkoutPlanExerciseRequestValidator : AbstractValidator<UpdateWorkoutPlanExerciseRequest>
{
    public UpdateWorkoutPlanExerciseRequestValidator()
    {
        RuleFor(x => x.Reps)
            .NotEmpty().WithMessage("Reps is required.")
            .InclusiveBetween(1,100).WithMessage("Reps must be between 1 and 100.");

            RuleFor(x => x.Sets)
            .NotEmpty().WithMessage("Sets is required.")
            .InclusiveBetween(1, 100).WithMessage("Sets must be between 1 and 100.");

        RuleFor(x => x.RestTime)
            .NotEmpty().WithMessage("Rest time is required.")
            .InclusiveBetween(0, 3600).WithMessage("Rest time must be between 0 and 3600 seconds.");

        RuleFor(x => x.Weight)
            .NotEmpty().WithErrorCode("Weight is required.")    
            .GreaterThanOrEqualTo(0).WithMessage("Weight must be greater than or equal to 0.");
    }
}
