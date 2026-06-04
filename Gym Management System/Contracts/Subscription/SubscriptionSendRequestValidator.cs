namespace Gym_Management_System.Contracts.Subscription;

public class SubscriptionSendRequestValidator : AbstractValidator<SubscriptionSendRequest>
{
    public SubscriptionSendRequestValidator()
    {
        RuleFor(x => x.MembershipPlanId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Invalid membership plan.");
    }
}

