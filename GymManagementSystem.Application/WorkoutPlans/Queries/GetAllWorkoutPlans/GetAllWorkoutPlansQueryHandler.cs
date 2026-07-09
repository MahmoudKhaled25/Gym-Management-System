using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.WorkoutPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Queries.GetAllWorkoutPlans;

public class GetAllWorkoutPlansQueryHandler(IWorkoutPlanQueries workoutPlanQueries) : IRequestHandler<GetAllWorkoutPlansQuery, Result<PaginatedList<WorkoutPlanDto>>>
{
    private readonly IWorkoutPlanQueries _workoutPlanQueries = workoutPlanQueries;

    public async Task<Result<PaginatedList<WorkoutPlanDto>>> Handle(GetAllWorkoutPlansQuery request, CancellationToken cancellationToken)
    {
        var workoutPlans = await _workoutPlanQueries.GetAllAsync(request.Filters,request.TrainerId, cancellationToken);
        return Result.Success(workoutPlans);
    }
}
