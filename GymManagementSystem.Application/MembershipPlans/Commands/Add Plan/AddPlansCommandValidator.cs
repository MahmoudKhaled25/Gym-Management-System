using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Commands.Add_Plan;

public class AddPlansCommandValidator : AbstractValidator<AddPlansCommand>
{
    public AddPlansCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(3, 100);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(3, 500);

        RuleFor(x => x.DurationInDays)
            .InclusiveBetween(1, 3650);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThanOrEqualTo(100000)
            .PrecisionScale(10, 2, true);
    }
}
