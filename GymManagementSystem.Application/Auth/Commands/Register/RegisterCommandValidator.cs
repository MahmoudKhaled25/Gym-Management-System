using FluentValidation;
using GymManagementSystem.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
           .Matches(RegexPatterns.PhoneNumber)
           .WithMessage("Invalid Egyptian phone number.");

        RuleFor(x => x.Gender)
            .IsInEnum();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password);
    }
}
