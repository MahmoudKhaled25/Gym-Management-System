
namespace Gym_Management_System.Contracts.Account;

public class UpdateUserProfileRequestValidator : AbstractValidator<UpdateUserProfileRequest>
{
    public UpdateUserProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.")
                        .Must(desc => !string.IsNullOrWhiteSpace(desc));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.")
            .Must(desc => !string.IsNullOrWhiteSpace(desc));

        RuleFor(x => x.DateOfBirth)
             .NotEmpty().WithMessage("Date of birth is required.")
             .Must(date => date < DateOnly.FromDateTime(DateTime.UtcNow))
             .WithMessage("Date of birth must be in the past.");
        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0.");
        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0.");

    }
}
