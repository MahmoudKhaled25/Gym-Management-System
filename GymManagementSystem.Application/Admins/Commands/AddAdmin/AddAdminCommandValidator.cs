using FluentValidation;
using GymManagementSystem.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Commands.AddAdmin;

public class AddAdminCommandValidator : AbstractValidator<AddAdminCommand>
{
    public AddAdminCommandValidator()
    {
        RuleFor(x => x.FirstName)
    .NotEmpty()
    .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(RegexPatterns.Password);

        RuleFor(x => x.PhoneNumber)
            .Matches(RegexPatterns.PhoneNumber)
            .WithMessage("Invalid Egyptian phone number.")
            .When(x => x.PhoneNumber is not null);

        RuleFor(x => x.Gender)
            .IsInEnum();
    }
}
