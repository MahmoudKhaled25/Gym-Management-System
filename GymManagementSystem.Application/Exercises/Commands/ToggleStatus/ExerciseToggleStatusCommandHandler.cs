using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Exercises.Commands.ToggleStatus;

public class ExerciseToggleStatusCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : IRequestHandler<ExerciseToggleStatusCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _exercisesCacheKey = "exercises_cache";

    public async Task<Result> Handle(ExerciseToggleStatusCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _unitOfWork.Repository<Exercise>().GetByIdAsync(request.Id);

        if (exercise is null)
            return Result.Failure(ExerciseErrors.ExerciseNotFound);

        exercise.IsActive = !exercise.IsActive;

        _unitOfWork.Repository<Exercise>().Update(exercise);

        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        _memoryCache.Remove(_exercisesCacheKey);

        return Result.Success();
        
 
    }
}
