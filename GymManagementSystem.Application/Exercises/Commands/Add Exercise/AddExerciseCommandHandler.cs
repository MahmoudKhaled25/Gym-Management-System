using FluentValidation.Results;
using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GymManagementSystem.Application.Exercises.Commands.Add_Exercise;

public class AddExerciseCommandHandler(IUnitOfWork unitOfWork,IMemoryCache memoryCache) : IRequestHandler<AddExerciseCommand, Result<ExerciseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _exercisesCacheKey = "exercises_cache";

    public async Task<Result<ExerciseDto>> Handle(AddExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = request.Adapt<Exercise>();
        try
        {
            await _unitOfWork.Repository<Exercise>().AddAsync(exercise);
            await _unitOfWork.SaveChangesAsync();
            _memoryCache.Remove(_exercisesCacheKey);

            var exerciseDto = exercise.Adapt<ExerciseDto>();
            return Result.Success(exerciseDto);

        }
        catch (DbUpdateException)
        {
            return Result.Failure<ExerciseDto>(ExerciseErrors.ExerciseExists);
        }
     
    }


}

