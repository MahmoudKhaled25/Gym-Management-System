using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Exercises.Commands.Add_Exercise;

public class AddExerciseCommandValidator : AbstractValidator<AddExerciseCommand>
{
    public AddExerciseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Description)
           .NotEmpty()
           .Length(3, 500);

        RuleFor(x => x.MuscleGroup)
           .NotEmpty()
           .Length(3, 100);
    }
}
