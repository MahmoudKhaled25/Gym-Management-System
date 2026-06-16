using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace GymManagementSystem.Application.Exercises.Queries.Get_All_Exercises;

public class GetAllExercisesQueryHandler(
    IMemoryCache memoryCache,
    IExerciseQueries exerciseQueries) : IRequestHandler<GetAllExercisesQuery, Result<PaginatedList<ExerciseDto>>>
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IExerciseQueries _exerciseQueries = exerciseQueries;
    

    public async Task<Result<PaginatedList<ExerciseDto>>> Handle(GetAllExercisesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey =
            $"Exercises_{request.Filters.PageNumber}_" +
            $"{request.Filters.PageSize}_" +
            $"{request.Filters.SearchValue}_" +
            $"{request.Filters.SortColumn}_" +
            $"{request.Filters.SortDirection}";

        if (_memoryCache.TryGetValue(cacheKey,out PaginatedList<ExerciseDto>? cached))
        {
            return Result.Success(cached!);
        }

        var result = await _exerciseQueries.GetAllAsync(request.Filters,cancellationToken);

        _memoryCache.Set(cacheKey,result,TimeSpan.FromMinutes(30));

        return Result.Success(result);
    }

}

