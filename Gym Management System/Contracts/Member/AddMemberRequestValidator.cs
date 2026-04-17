using Gym_Management_System.Abstractions.Consts;

namespace Gym_Management_System.Contracts.Member;

public class AddMemberRequestValidator : AbstractValidator<AddMemberRequest>
{
    public AddMemberRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                    .Matches(RegexPatterns.Password).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        
         

        RuleFor(x => x.FirstName)
           .NotEmpty().WithMessage("First name is required.")
                       .Must(name => !string.IsNullOrWhiteSpace(name))
           .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
                        .Must(name => !string.IsNullOrWhiteSpace(name))
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

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
