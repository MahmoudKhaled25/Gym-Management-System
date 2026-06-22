using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Commands.DeleteWorkoutPlan;

public record DeleteWorkoutPlanCommand(int Id) : IRequest<Result>;
