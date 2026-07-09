using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Notifications.Commands;

public class SendOffersCommandValidator : AbstractValidator<SendOffersCommand>
{
    public SendOffersCommandValidator()
    {
        RuleFor(x => x.Message)
           .NotEmpty().WithMessage("Message is required.")
           .MaximumLength(500).WithMessage("Message cannot exceed 500 characters.");
    }
}
