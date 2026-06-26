using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Queries.GetActiveTrainers;

public class GetActiveTrainersQueryHandler(ITrainerQueries trainerQueries,IMemoryCache memoryCache) : IRequestHandler<GetActiveTrainersQuery, Result<IEnumerable<TrainerDto>>>
{
    private readonly ITrainerQueries _trainerQueries = trainerQueries;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _activeTrainersCacheKey = "Trainers_Active";

    public async Task<Result<IEnumerable<TrainerDto>>> Handle(GetActiveTrainersQuery request, CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(_activeTrainersCacheKey, out IEnumerable<TrainerDto>? cached))
            return Result.Success(cached!);

        var activeTrainers = await _trainerQueries.GetActiveAsync(cancellationToken);
        _memoryCache.Set(_activeTrainersCacheKey, activeTrainers, TimeSpan.FromMinutes(30));
        return Result.Success(activeTrainers);

    }
}
