using FluentValidation;
using Gym_Management_System.Abstractions.Consts;

namespace Gym_Management_System.Contracts.Auth;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();    


        RuleFor(x => x.Code)
            .NotEmpty();


        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password Should be a least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase."); ;
    }
}
