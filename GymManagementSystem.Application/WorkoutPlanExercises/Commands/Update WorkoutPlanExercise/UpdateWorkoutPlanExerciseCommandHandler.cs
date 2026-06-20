using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlanExercises.Commands.Update_WorkoutPlanExercise;

public class UpdateWorkoutPlanExerciseCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateWorkoutPlanExerciseCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateWorkoutPlanExerciseCommand request, CancellationToken cancellationToken)
    {
        var workoutPlanExercise = await _unitOfWork.Repository<WorkoutPlanExercise>()
            .Query()
            .SingleOrDefaultAsync(x => x.Id == request.workoutPlanExerciseId, cancellationToken);

        if (workoutPlanExercise == null)
            return Result.Failure(WorkoutPlanExerciseErrors.WorkoutPlanExerciseNotFound);

        request.Adapt(workoutPlanExercise);
        _unitOfWork.Repository<WorkoutPlanExercise>().Update(workoutPlanExercise);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
