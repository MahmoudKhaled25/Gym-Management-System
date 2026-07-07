using FluentValidation;
using GymManagementSystem.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
          .NotEmpty()
          .EmailAddress();


        RuleFor(x => x.Code)
            .NotEmpty();


        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password Should be a least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase.");
    }
}
