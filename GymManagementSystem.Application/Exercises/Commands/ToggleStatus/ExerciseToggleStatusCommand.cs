using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Exercises.Commands.ToggleStatus;

public record ExerciseToggleStatusCommand(int Id) : IRequest<Result>;

