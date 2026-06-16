using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Exercises.Commands;

public record UpdateExerciseCommand(int Id,string Name, string Description, string MuscleGroup) : IRequest<Result>;

