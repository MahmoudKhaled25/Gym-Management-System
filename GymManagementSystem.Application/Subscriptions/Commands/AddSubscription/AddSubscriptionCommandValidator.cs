using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Commands.AddSubscription;

public class AddSubscriptionCommandValidator : AbstractValidator<AddSubscriptionCommand>
{
    public AddSubscriptionCommandValidator()
    {
        RuleFor(x => x.MembershipPlanId)
         .NotEmpty()
         .GreaterThan(0).WithMessage("Invalid membership plan.");
    }
}
