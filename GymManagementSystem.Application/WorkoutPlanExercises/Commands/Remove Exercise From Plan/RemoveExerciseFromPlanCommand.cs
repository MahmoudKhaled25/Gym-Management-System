using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlanExercises.Commands.Remove_Exercise_From_Plan;

public record RemoveExerciseFromPlanCommand(int WorkoutPlanExerciseId) : IRequest<Result>;
