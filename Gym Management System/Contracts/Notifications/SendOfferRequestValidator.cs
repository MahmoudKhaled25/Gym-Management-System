namespace GymManagementSystem.Contracts.Notifications;

public class SendOfferRequestValidator : AbstractValidator<SendOfferRequest>
{
    public SendOfferRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(500).WithMessage("Message cannot exceed 500 characters.");
    }
}