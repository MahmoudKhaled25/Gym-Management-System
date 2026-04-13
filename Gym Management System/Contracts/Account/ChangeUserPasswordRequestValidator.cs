using Gym_Management_System.Abstractions.Consts;

namespace Gym_Management_System.Contracts.Account;

public class ChangeUserPasswordRequestValidator : AbstractValidator<ChangeUserPasswordRequest>
{
    public ChangeUserPasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
            .Matches(RegexPatterns.Password).WithMessage("New password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

        RuleFor(x => x.OldPassword)
            .NotEqual(x => x.NewPassword).WithMessage("New password must be different from the old password.");
    }
}
