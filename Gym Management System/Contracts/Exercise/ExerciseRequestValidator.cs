namespace Gym_Management_System.Contracts.Exercise;

public class ExerciseRequestValidator : AbstractValidator<ExerciseRequest>
{
    public ExerciseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Description)
           .NotEmpty()
           .Length(3, 500);

        RuleFor(x => x.MuscleGroup)
           .NotEmpty()
           .Length(3, 100);
    }
}
