using FluentValidation;
using Gym_Management_System.Abstractions.Consts;

namespace Gym_Management_System.Contracts.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Must(desc => !string.IsNullOrWhiteSpace(desc))
            .Length(3, 100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Must(desc => !string.IsNullOrWhiteSpace(desc))
            .Length(3, 100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password);

    }
}
