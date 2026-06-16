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
            .Where(x =>
                string.IsNullOrEmpty(filters.SearchValue) ||
                x.Name.Contains(filters.SearchValue) ||
                x.Description.Contains(filters.SearchValue) ||
                x.MuscleGroup.Contains(filters.SearchValue));

        var sortedQuery = query.ApplySort(
            filters.SortColumn,
            filters.SortDirection);

        return await PaginatedList<ExerciseDto>.CreateAsync(
            sortedQuery.ProjectToType<ExerciseDto>(),
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);
    }
    }