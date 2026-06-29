using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Application.Members.Queries;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Consts;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Infrastructure.Queries;

public class MemberQueries(ApplicationDbContext context) : IMemberQueries
{
    private readonly ApplicationDbContext _context = context;

    public async Task<PaginatedList<MemberSummaryDto>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken)
    {
        var query = _context.Users
           .Where(u => _context.UserRoles
          .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
          .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name) &&
                  (string.IsNullOrEmpty(filters.SearchValue) ||
                   u.FirstName.Contains(filters.SearchValue) ||
                   u.LastName.Contains(filters.SearchValue) ||
                   u.Email!.Contains(filters.SearchValue)));

        var sortedQuery = filters.SortColumn?.ToLower() switch
        {
            "fullname" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.FirstName)
                : query.OrderBy(u => u.FirstName),

            "email" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),

            "gender" => filters.SortDirection == "DESC"
                  ? query.OrderByDescending(u => u.Gender)
                  : query.OrderBy(u => u.Gender),

            _ => query.OrderBy(u => u.FirstName)
        };

        var finalQuery = sortedQuery.Select(u => new MemberSummaryDto(
            u.Id,
            $"{u.FirstName} {u.LastName}",
            u.Email!,
            u.PhoneNumber,
            u.Gender,
            u.Trainer == null ? null : $"{u.Trainer.ApplicationUser!.FirstName} {u.Trainer.ApplicationUser.LastName}",
            u.Subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active)
                .Select(s => s.MembershipPlan!.Name)
                .FirstOrDefault(),
            u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow
        ));

        var result = await PaginatedList<MemberSummaryDto>.CreateAsync(
            finalQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

        return result;
    }
    public async Task<PaginatedList<MemberSummaryDto>> GetActiveAsync(RequestFilters filters, CancellationToken cancellationToken)
    {
        var query = _context.Users
    .Where(u => _context.UserRoles
     .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
     .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name) &&
             (string.IsNullOrEmpty(filters.SearchValue) ||
              u.FirstName.Contains(filters.SearchValue) ||
              u.LastName.Contains(filters.SearchValue) ||
              u.Email!.Contains(filters.SearchValue)) && (u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow));

        var sortedQuery = filters.SortColumn?.ToLower() switch
        {
            "fullname" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.FirstName)
                : query.OrderBy(u => u.FirstName),

            "email" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),

            "gender" => filters.SortDirection == "DESC"
          ? query.OrderByDescending(u => u.Gender)
          : query.OrderBy(u => u.Gender),

            _ => query.OrderBy(u => u.FirstName)
        };

        var finalQuery = sortedQuery.Select(u => new MemberSummaryDto(
            u.Id,
            $"{u.FirstName} {u.LastName}",
            u.Email!,
            u.PhoneNumber,
            u.Gender,
            u.Trainer == null ? null : $"{u.Trainer.ApplicationUser!.FirstName} {u.Trainer.ApplicationUser.LastName}",
            u.Subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active)
                .Select(s => s.MembershipPlan!.Name)
                .FirstOrDefault(),
            u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow
        ));

        var result = await PaginatedList<MemberSummaryDto>.CreateAsync(
            finalQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

        return result;
    }

    public async Task<MemberSummaryDto?> GetByIdAsync(string memberId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
             .Where(u => u.Id == memberId &&
                         _context.UserRoles
                             .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                             .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name))
             .Select(u => new MemberSummaryDto(
                 u.Id,
                 $"{u.FirstName} {u.LastName}",
                 u.Email!,
                 u.PhoneNumber,
                 u.Gender,
                 u.Trainer == null ? null : $"{u.Trainer.ApplicationUser!.FirstName} {u.Trainer.ApplicationUser.LastName}",
                 u.Subscriptions
                     .Where(s => s.Status == SubscriptionStatus.Active)
                     .Select(s => s.MembershipPlan!.Name)
                     .FirstOrDefault(),
                 u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow
             ))
             .FirstOrDefaultAsync(cancellationToken);
    }
}
