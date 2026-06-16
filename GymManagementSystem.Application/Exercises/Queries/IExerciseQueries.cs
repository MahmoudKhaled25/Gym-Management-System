using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Exercises.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Exercises.Queries;

public interface IExerciseQueries
{
    Task<PaginatedList<ExerciseDto>> GetAllAsync(
     RequestFilters filters,
     CancellationToken cancellationToken = default);
}
