using GymManagementSystem.Application.WorkoutPlanExercises.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Application.WorkoutPlanExercises.Queries.GetWorkoutPlanExercise;

public class GetWorkoutPlanExerciseQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetWorkoutPlanExerciseQuery, Result<WorkoutPlanExercisesGroupedDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<WorkoutPlanExercisesGroupedDto>> Handle(GetWorkoutPlanExerciseQuery request, CancellationToken cancellationToken)
    {
        var exercises = await _unitOfWork.Repository<WorkoutPlanExercise>()
            .Query()
            .Where(x => x.WorkoutPlanId == request.WorkoutPlanId)
    .Select(x => new
    {
        WorkoutPlanName = x.WorkoutPlan!.Name,
        Exercise = new WorkoutPlanExerciseDto(
            x.Id,
            x.Sets,
            x.Reps,
            x.Weight,
            x.RestTime,
            x.Exercise!.Name
        )
    })
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var grouped = new WorkoutPlanExercisesGroupedDto(
            exercises.First().WorkoutPlanName,
            exercises.Select(x => x.Exercise)
        );

        return Result.Success(grouped);
    }
}

