using GymManagementSystem.Application.Admins.Dtos;
using GymManagementSystem.Application.Admins.Queries;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Consts;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Infrastructure.Queries;

public class AdminQueries(ApplicationDbContext context) : IAdminQueries
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<AdminDto>>> GetAllAdminsAsync(CancellationToken cancellationToken = default)
    {
        var admins = await _context.Users
          .Where(u => _context.UserRoles
              .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
           .Any(ur => ur.UserId == u.Id && ur.Name == DefaultRoles.Admin.Name))
                          .AsNoTracking()
                          .Select(u => new AdminDto
                          (
                          u.Id,
                          u.FirstName + " " + u.LastName,
                          u.Email!,
                          u.PhoneNumber,
                          u.Gender,
                          u.LockoutEnd <= DateTimeOffset.UtcNow || u.LockoutEnd == null ? true : false
                          ))
                          .ToListAsync(cancellationToken);

        return Result.Success(admins.AsEnumerable());
    }

    public async Task<Result<DashboardDataDto>> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var totalMembers = await _context.Users
          .Where(u => _context.UserRoles
              .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
              .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name))
          .CountAsync(cancellationToken);

        var activeMembers = await _context.Users
          .Where(u => _context.UserRoles
              .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
              .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name)
              && (u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow))
          .CountAsync(cancellationToken);

        var totalTrainers = await _context.Trainers.CountAsync(cancellationToken);
        var activeTrainers = await _context.Trainers
          .Where(t => t.IsActive)
          .CountAsync(cancellationToken);

        var totalSubscriptions = await _context.Subscriptions.CountAsync(cancellationToken);
        var activeSubscriptions = await _context.Subscriptions
          .Where(s => s.Status == SubscriptionStatus.Active)
          .CountAsync(cancellationToken);

        var pendingRequests = await _context.SubscriptionRequests
            .Where(r => r.Status == SubscriptionRequestStatus.Pending)
            .CountAsync(cancellationToken);

        var totalRevenue = await _context.Subscriptions
                 .Where(s => s.Status != SubscriptionStatus.Cancelled)
                 .SumAsync(s => s.MembershipPlan!.Price, cancellationToken);

        var subscriptionsByPlan = await _context.Subscriptions
            .Where(s => s.Status == SubscriptionStatus.Active)
                    .GroupBy(s => s.MembershipPlan!.Name)
                    .Select(g => new PlanSubscriptionsCount
                    (
                        g.Key,
                        g.Count()
                    ))
                    .ToListAsync(cancellationToken);

        var newMembersPerMonth = await _context.Users
            .Where(u => _context.UserRoles
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name))
            .GroupBy(x => x.CreatedAt.Month)
            .Select(g => new MonthlyCount
            (
                Month: g.Key.ToString(),
                Count: g.Count()
            )).ToListAsync(cancellationToken);

        var revenuePerMonth = await _context.Subscriptions
        .Where(s => s.StartDate.Year == DateTime.UtcNow.Year
                 && s.Status != SubscriptionStatus.Cancelled)
        .GroupBy(s => s.StartDate.Month)
        .Select(g => new MonthlyRevenue(
            g.Key.ToString(),
            g.Sum(s => s.MembershipPlan!.Price)))
        .ToListAsync(cancellationToken);


        return Result.Success(new DashboardDataDto(
       totalMembers,
       activeMembers,
       totalTrainers,
       activeTrainers,
       totalSubscriptions,
       activeSubscriptions,
       pendingRequests,
       totalRevenue,
       subscriptionsByPlan,
       newMembersPerMonth,
       revenuePerMonth
   ));
    }
}
