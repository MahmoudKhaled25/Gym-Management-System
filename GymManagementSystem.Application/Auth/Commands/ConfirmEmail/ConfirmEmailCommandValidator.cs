using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.ConfirmEmail;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Email)
           .EmailAddress()
           .NotEmpty().WithMessage("Email is required.");
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required.");
    }
}
