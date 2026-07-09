using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.WorkoutPlans.Dtos;
using GymManagementSystem.Application.WorkoutPlans.Queries;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Infrastructure.Queries;

public class WorkoutPlanQueries(ApplicationDbContext context) : IWorkoutPlanQueries
{
    private readonly ApplicationDbContext _context = context;

    public async Task<PaginatedList<WorkoutPlanDto>> GetAllAsync(RequestFilters filters, string? trainerId, CancellationToken cancellationToken = default)
    {
        var query = _context.WorkoutPlans
           .Where(x =>
               (trainerId == null || x.TrainerId == trainerId) &&
               (string.IsNullOrEmpty(filters.SearchValue) ||
                x.Name.Contains(filters.SearchValue) ||
                x.Description.Contains(filters.SearchValue) ||

                (x.Trainer != null &&
                 x.Trainer.ApplicationUser != null &&
                 x.Trainer.ApplicationUser.FirstName.Contains(filters.SearchValue)) ||

                (x.Trainer != null &&
                 x.Trainer.ApplicationUser != null &&
                 x.Trainer.ApplicationUser.LastName.Contains(filters.SearchValue)) ||

                (x.User != null &&
                 x.User.FirstName.Contains(filters.SearchValue)) ||

                (x.User != null &&
                 x.User.LastName.Contains(filters.SearchValue))
               ));

        query = filters.SortColumn?.ToLower() switch
        {
            "membername" => filters.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.User!.FirstName)
                       .ThenByDescending(x => x.User!.LastName)
                : query.OrderBy(x => x.User!.FirstName)
                       .ThenBy(x => x.User!.LastName),

            "trainername" => filters.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.Trainer!.ApplicationUser!.FirstName)
                       .ThenByDescending(x => x.Trainer!.ApplicationUser!.LastName)
                : query.OrderBy(x => x.Trainer!.ApplicationUser!.FirstName)
                       .ThenBy(x => x.Trainer!.ApplicationUser!.LastName),

            "name" => filters.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name),

            _ => query.OrderBy(x => x.Id)
        };

        var projectedQuery = query.Select(x => new WorkoutPlanDto(
            x.Id,
            x.Name,
            x.Description,

            x.Trainer == null
                ? null
                : $"{x.Trainer.ApplicationUser!.FirstName} {x.Trainer.ApplicationUser.LastName}",

            $"{x.User!.FirstName} {x.User.LastName}"
        ));

        var result = await PaginatedList<WorkoutPlanDto>.CreateAsync(
            projectedQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

        return result;
    }
}
