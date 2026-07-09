using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Commands.DeleteWorkoutPlan;

public class DeleteWorkoutPlanCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteWorkoutPlanCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var workoutPlan = await _unitOfWork.Repository<WorkoutPlan>()
          .Query()
          .Include(x => x.WorkoutPlanExercises)
          .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (workoutPlan is null)
            return Result.Failure(WorkoutPlanErrors.WorkoutPlanNotFound);

        _unitOfWork.Repository<WorkoutPlanExercise>().DeleteRange(workoutPlan.WorkoutPlanExercises);
        _unitOfWork.Repository<WorkoutPlan>().Delete(workoutPlan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
