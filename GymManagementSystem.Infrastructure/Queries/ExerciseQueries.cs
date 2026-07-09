using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Application.Exercises.Queries;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Infrastructure.Queries;

public class ExerciseQueries(ApplicationDbContext context) : IExerciseQueries
{
    private readonly ApplicationDbContext _context = context;

    public async Task<PaginatedList<ExerciseDto>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.Exercises
            .AsNoTracking() 
            .Where(x => string.IsNullOrWhiteSpace(filters.SearchValue) ||
                        x.Name.Contains(filters.SearchValue) ||
                        x.Description.Contains(filters.SearchValue) ||
                        x.MuscleGroup.Contains(filters.SearchValue));

        query = filters.SortColumn?.ToLower() switch
        {
            "name" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),

            "description" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.Description)
                : query.OrderByDescending(x => x.Description),

            "musclegroup" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.MuscleGroup)
                : query.OrderByDescending(x => x.MuscleGroup),

            "id" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.Id)
                : query.OrderByDescending(x => x.Id),

            _ => query.OrderBy(x => x.Name) 
        };

        var dtoQuery = query.Select(x => new ExerciseDto(
            x.Id,
            x.Name,
            x.Description,
            x.MuscleGroup
        ));

        return await PaginatedList<ExerciseDto>.CreateAsync(
            dtoQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);
    }
}