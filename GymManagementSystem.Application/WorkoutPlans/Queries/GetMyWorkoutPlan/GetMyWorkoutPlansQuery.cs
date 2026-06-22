using GymManagementSystem.Application.WorkoutPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Queries.GetMyWorkoutPlans;

public record GetMyWorkoutPlansQuery(string UserId): IRequest<Result<IEnumerable<WorkoutPlanGroupedDto>>>;