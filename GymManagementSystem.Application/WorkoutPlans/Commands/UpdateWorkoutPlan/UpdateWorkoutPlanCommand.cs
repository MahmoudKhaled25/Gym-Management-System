using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Commands.UpdateWorkoutPlan;

public record UpdateWorkoutPlanCommand(
    int Id,
    string Name,
    string Description,
    string MemberId,
    string? TrainerId) : IRequest<Result>;
