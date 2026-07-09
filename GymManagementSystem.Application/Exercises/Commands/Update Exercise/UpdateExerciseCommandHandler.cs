using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GymManagementSystem.Application.Exercises.Commands.Update_Exercise;

public class UpdateExerciseCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : IRequestHandler<UpdateExerciseCommand,Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _exercisesCacheKey = "exercises_cache";

    public async Task<Result> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _unitOfWork.Repository<Exercise>()
            .GetByIdAsync(request.Id);

        if (exercise is null)
            return Result.Failure(ExerciseErrors.ExerciseNotFound);

        request.Adapt(exercise);

        _unitOfWork.Repository<Exercise>().Update(exercise);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _memoryCache.Remove(_exercisesCacheKey);
            return Result.Success();
        }
        catch (DbUpdateException)
        {
            return Result.Failure(ExerciseErrors.ExerciseExists);
        }
    }

}
