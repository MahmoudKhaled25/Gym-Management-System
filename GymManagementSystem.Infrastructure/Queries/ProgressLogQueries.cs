using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Application.ProgressLogs.Dtos;
using GymManagementSystem.Application.ProgressLogs.Queries;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Infrastructure.Queries;

public class ProgressLogQueries(ApplicationDbContext context) : IProgressLogQueries
{
    private readonly ApplicationDbContext _context = context;

    public async Task<PaginatedList<AllProgressLogsDto>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.ProgressLogs
            .AsNoTracking() 
            .Where(x => string.IsNullOrWhiteSpace(filters.SearchValue) ||
                        x.Notes.Contains(filters.SearchValue) ||
                        x.Weight.ToString().Contains(filters.SearchValue) ||
                        (x.User != null && x.User.FirstName.Contains(filters.SearchValue)) ||
                        (x.User != null && x.User.LastName.Contains(filters.SearchValue)));

        query = filters.SortColumn?.ToLower() switch
        {
            "memberfullname" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.User!.FirstName).ThenBy(x => x.User!.LastName)
                : query.OrderByDescending(x => x.User!.FirstName).ThenByDescending(x => x.User!.LastName),

            "weight" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.Weight)
                : query.OrderByDescending(x => x.Weight),

            "logdate" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.LogDate)
                : query.OrderByDescending(x => x.LogDate),

            _ => query.OrderByDescending(x => x.LogDate) 
        };

        var dtoQuery = query.Select(x => new AllProgressLogsDto(
            x.Id,
            x.User != null ? $"{x.User.FirstName} {x.User.LastName}" : string.Empty,
            x.Weight,
            x.Notes,
            x.LogDate
        ));

        return await PaginatedList<AllProgressLogsDto>.CreateAsync(
            dtoQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);
    }

    public async Task<PaginatedList<MyProgressLogsDto>> GetMyAsync(string userId, RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.ProgressLogs
            .AsNoTracking()
            .Where(x => x.UserId == userId &&
                        (string.IsNullOrWhiteSpace(filters.SearchValue) ||
                         x.Notes.Contains(filters.SearchValue) ||
                         x.Weight.ToString().Contains(filters.SearchValue) ||
                         x.LogDate.ToString().Contains(filters.SearchValue)));

        query = filters.SortColumn?.ToLower() switch
        {
            "weight" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.Weight)
                : query.OrderByDescending(x => x.Weight),

            "notes" => filters.SortDirection?.ToLower() == "asc"
            ? query.OrderBy(x => x.Notes)
            : query.OrderByDescending(x => x.Notes),

            "logdate" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.LogDate)
                : query.OrderByDescending(x => x.LogDate),

            _ => query.OrderByDescending(x => x.LogDate) 
        };

        var dtoQuery = query.Select(x => new MyProgressLogsDto(
            x.Weight,
            x.Notes,
            x.LogDate
        ));

 

        // 4. تنفيذ الـ Pagination
        return await PaginatedList<MyProgressLogsDto>.CreateAsync(
            dtoQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);
    }
}
