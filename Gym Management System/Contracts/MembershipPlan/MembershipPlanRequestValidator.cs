namespace Gym_Management_System.Contracts.MembershipPlan;

public class MembershipPlanRequestValidator : AbstractValidator<MembershipPlanRequest>
{
    public MembershipPlanRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .Length(3, 100);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Must(desc => !string.IsNullOrWhiteSpace(desc))
            .Length(3, 500);

        RuleFor(x => x.DurationInDays)
            .InclusiveBetween(1, 3650);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThanOrEqualTo(100000)
            .PrecisionScale(10, 2, true);
    }
}
