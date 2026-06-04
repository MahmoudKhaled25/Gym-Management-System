namespace GymManagementSystem.Contracts.SubscriptionRequest;

public class SubscriptionRequestSendRequestValidator : AbstractValidator<SubscriptionRequestSendRequest>
{
    public SubscriptionRequestSendRequestValidator()
    {
        RuleFor(x => x.MembershipPlanId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Invalid membership plan.");

    }
}
