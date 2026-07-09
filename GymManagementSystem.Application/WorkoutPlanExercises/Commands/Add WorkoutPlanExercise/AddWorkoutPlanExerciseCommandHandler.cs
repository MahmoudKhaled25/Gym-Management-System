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

namespace GymManagementSystem.Application.WorkoutPlanExercises.Commands.Add_WorkoutPlanExercise;

public class AddWorkoutPlanExerciseCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddWorkoutPlanExerciseCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddWorkoutPlanExerciseCommand request, CancellationToken cancellationToken)
    {
        var IsWorkoutPlanExist = await _unitOfWork.Repository<WorkoutPlan>()
            .Query()
            .AnyAsync(x => x.Id == request.WorkoutPlanId, cancellationToken);
        if (!IsWorkoutPlanExist)
            return Result.Failure(WorkoutPlanErrors.WorkoutPlanNotFound);
        
        var IsExerciseExist = await _unitOfWork.Repository<Exercise>()
            .Query()
            .AnyAsync(x => x.Id == request.ExerciseId, cancellationToken);
        if (!IsExerciseExist)
            return Result.Failure(ExerciseErrors.ExerciseNotFound);
        
        var workoutPlanExercise = request.Adapt<WorkoutPlanExercise>();
        workoutPlanExercise.WorkoutPlanId = request.WorkoutPlanId;

        await _unitOfWork.Repository<WorkoutPlanExercise>().AddAsync(workoutPlanExercise);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
