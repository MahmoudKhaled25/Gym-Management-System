using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Commands.UpdateWorkoutPlan;

public class UpdateWorkoutPlanCommandValidator : AbstractValidator<UpdateWorkoutPlanCommand>
{
    public UpdateWorkoutPlanCommandValidator()
    {
        RuleFor(x => x.Name)
   .NotEmpty().WithMessage("Workout plan name is required.")
   .Length(3, 100).WithMessage("Workout plan name must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Workout plan description is required.")
            .Length(3, 500).WithMessage("Workout plan description must be between 3 and 500 characters.");

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member ID is required.");
    }
}
