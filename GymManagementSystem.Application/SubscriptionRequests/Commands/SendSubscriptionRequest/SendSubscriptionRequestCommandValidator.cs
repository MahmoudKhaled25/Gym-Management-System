using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.SubscriptionRequests.Commands.SendSubscriptionRequest;

public class SendSubscriptionRequestCommandValidator : AbstractValidator<SendSubscriptionRequestCommand>
{
    public SendSubscriptionRequestCommandValidator()
    {
        RuleFor(x => x.MembershipPlanId)
       .NotEmpty()
       .GreaterThan(0).WithMessage("Invalid membership plan.");
    }
}
