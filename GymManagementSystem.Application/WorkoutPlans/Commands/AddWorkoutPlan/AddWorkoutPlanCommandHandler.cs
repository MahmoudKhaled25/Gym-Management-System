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

namespace GymManagementSystem.Application.WorkoutPlans.Commands.AddWorkoutPlan;

public class AddWorkoutPlanCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddWorkoutPlanCommand, Result>

{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateMemberAndTrainerAsync(
                            request.MemberId,
                            request.TrainerId,
                            cancellationToken);

        if (!validationResult.IsSuccess)
            return validationResult;

        var workoutPlan = request.Adapt<WorkoutPlan>();
        await _unitOfWork.Repository<WorkoutPlan>().AddAsync(workoutPlan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    private async Task<Result> ValidateMemberAndTrainerAsync(string memberId, string? trainerId, CancellationToken cancellationToken)
    {
        var memberExists = await _unitOfWork.Repository<ApplicationUser>()
            .Query()
            .AnyAsync(x => x.Id == memberId, cancellationToken);
        if (!memberExists)
            return Result.Failure(UserErrors.UserNotFound);

        if (trainerId is not null)
        {
            var trainerExists = await _unitOfWork.Repository<Trainer>()
                .Query()
                .AnyAsync(x => x.UserId == trainerId, cancellationToken);
            if (!trainerExists)
                return Result.Failure(TrainerErrors.TrainerNotFound);
        }

        return Result.Success();
    }

}