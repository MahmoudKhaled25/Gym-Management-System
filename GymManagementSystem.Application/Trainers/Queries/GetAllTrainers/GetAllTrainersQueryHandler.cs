using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Queries.GetAllTrainers;

public class GetAllTrainersQueryHandler(ITrainerQueries trainerQueries,IMemoryCache memoryCache) : IRequestHandler<GetAllTrainersQuery, Result<PaginatedList<TrainerDto>>>
{
    private readonly ITrainerQueries _trainerQueries = trainerQueries;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _allTrainersCacheKey = "Trainers_All";

    public async Task<Result<PaginatedList<TrainerDto>>> Handle(GetAllTrainersQuery request, CancellationToken cancellationToken)
    {
        var trainers = await _trainerQueries.GetAllAsync(request.Filters, cancellationToken);
        _memoryCache.Set(_allTrainersCacheKey, trainers, TimeSpan.FromMinutes(30));
        return Result.Success(trainers);
    }
}
