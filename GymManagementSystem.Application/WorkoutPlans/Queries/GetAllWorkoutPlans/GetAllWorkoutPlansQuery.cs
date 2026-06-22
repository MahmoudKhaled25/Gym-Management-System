using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.WorkoutPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Queries.GetAllWorkoutPlans;

public record GetAllWorkoutPlansQuery(RequestFilters Filters, string? TrainerId) : IRequest<Result<PaginatedList<WorkoutPlanDto>>>;
