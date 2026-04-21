namespace Gym_Management_System.Contracts.Subscription;

public class SubscriptionRequestValidator : AbstractValidator<SubscriptionRequest>
{
    public SubscriptionRequestValidator()
    {
        RuleFor(x => x.MembershipPlanId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Invalid membership plan.");
    }
}

