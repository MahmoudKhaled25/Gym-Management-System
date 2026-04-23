namespace Gym_Management_System.Contracts.Exercise;

public class ExerciseRequestValidator : AbstractValidator<ExerciseRequest>
{
    public ExerciseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(desc => !string.IsNullOrWhiteSpace(desc))
            .Length(3, 100);

        RuleFor(x => x.Description)
           .NotEmpty()
           .Must(desc => !string.IsNullOrWhiteSpace(desc))
           .Length(3, 500);

        RuleFor(x => x.MuscleGroup)
           .NotEmpty()
           .Must(desc => !string.IsNullOrWhiteSpace(desc))
           .Length(3, 100);
    }
}
