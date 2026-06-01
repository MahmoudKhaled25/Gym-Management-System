namespace GymManagementSystem.Contracts.Auth;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty().WithMessage("Email is required.");
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required.");
    }
}
