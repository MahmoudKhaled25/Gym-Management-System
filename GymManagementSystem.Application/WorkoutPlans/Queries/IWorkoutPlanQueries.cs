using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.ProgressLogs.Dtos;
using GymManagementSystem.Application.WorkoutPlans.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Queries;

public interface IWorkoutPlanQueries
{
    Task<PaginatedList<WorkoutPlanDto>> GetAllAsync(RequestFilters filters,string? trainerId, CancellationToken cancellationToken = default);

}
