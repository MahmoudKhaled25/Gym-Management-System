using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Commands.Add_ProgressLog;

public class AddProgressLogCommandValidator : AbstractValidator<AddProgressLogCommand>
{
    public AddProgressLogCommandValidator()
    {
        RuleFor(x => x.Weight)
           .GreaterThan(0).WithMessage("Weight must be greater than 0.");
        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        RuleFor(x => x.LogDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Log date cannot be in the future.");
    }
}
