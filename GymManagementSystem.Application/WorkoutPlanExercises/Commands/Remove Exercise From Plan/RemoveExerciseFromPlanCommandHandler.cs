using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlanExercises.Commands.Remove_Exercise_From_Plan;

public class RemoveExerciseFromPlanCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveExerciseFromPlanCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RemoveExerciseFromPlanCommand request, CancellationToken cancellationToken)
    {
        var workoutPlanExercise = await _unitOfWork
              .Repository<WorkoutPlanExercise>()
              .GetByIdAsync(request.WorkoutPlanExerciseId);

        if (workoutPlanExercise is null)
            return Result.Failure(WorkoutPlanExerciseErrors.WorkoutPlanExerciseNotFound);

        _unitOfWork.Repository<WorkoutPlanExercise>().Delete(workoutPlanExercise);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
