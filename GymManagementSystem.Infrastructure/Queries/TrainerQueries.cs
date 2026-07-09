using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Application.Trainers.Queries;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Infrastructure.Queries;

public class TrainerQueries(ApplicationDbContext context) : ITrainerQueries
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<TrainerDto>> GetActiveAsync(CancellationToken cancellationToken)
    {
        var activeTrainers = await _context.Trainers
            .Where(x => x.IsActive)
             .Select(x => new
             {
                 x.UserId,
                 x.ApplicationUser!.FirstName,
                 x.ApplicationUser.LastName,
                 x.Specialization,
                 x.IsActive,
                 Roles = _context.UserRoles.Where(ur => ur.UserId == x.UserId)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToList()
             }).ToListAsync();

        var response = activeTrainers.Select(t => new TrainerDto(
           t.UserId,
           t.FirstName,
           t.LastName,
           t.Specialization,
           t.IsActive,
           t.Roles!
       ));

        return response;


    }

    public async Task<PaginatedList<TrainerDto>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken)
    {
        var query = _context.Users
            .Where(u => u.Trainer != null &&
                        (string.IsNullOrEmpty(filters.SearchValue) ||
                         u.FirstName.Contains(filters.SearchValue) ||
                         u.LastName.Contains(filters.SearchValue) ||
                         u.Email!.Contains(filters.SearchValue)));

        query = filters.SortColumn?.ToLower() switch
        {
            "firstname" => filters.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.FirstName!)
                : query.OrderBy(x => x.FirstName),

            "lastname" => filters.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.LastName)
                : query.OrderBy(x => x.LastName),

            "specialization" => filters.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.Trainer!.Specialization)
                : query.OrderBy(x => x.Trainer!.Specialization),

            "isactive" => filters.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.Trainer!.IsActive)
                : query.OrderBy(x => x.Trainer!.IsActive),

            _ => query.OrderBy(x => x.FirstName)
        };

        var finalQuery = query.Select(u => new TrainerDto(
            u.Id,
            u.FirstName,
            u.LastName,
            u.Trainer!.Specialization,
            u.Trainer.IsActive,
            _context.UserRoles
                .Where(ur => ur.UserId == u.Id)
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .ToList()!
        ));

        var result = await PaginatedList<TrainerDto>.CreateAsync(
            finalQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

      

        return result;
    }
}
