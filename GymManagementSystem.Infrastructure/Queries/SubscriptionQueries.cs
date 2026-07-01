using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Subscriptions.Dtos;
using GymManagementSystem.Application.Subscriptions.Queries;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Infrastructure.Queries;

public class SubscriptionQueries(ApplicationDbContext context) : ISubscriptionQueries
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<PaginatedList<SubscriptionDto>>> GetAllSubscriptionsAsync(RequestFilters filters, CancellationToken cancellationToken)
    {
        var query = _context.Subscriptions
           .Where(s => string.IsNullOrEmpty(filters.SearchValue) ||
               s.User!.FirstName.Contains(filters.SearchValue) ||
               s.User!.LastName.Contains(filters.SearchValue) ||
               s.Trainer!.ApplicationUser!.FirstName.Contains(filters.SearchValue) ||
               s.Trainer.ApplicationUser!.LastName.Contains(filters.SearchValue) ||
               s.MembershipPlan!.Name.Contains(filters.SearchValue));


        var sortedQuery = filters.SortColumn?.ToLower() switch
        {
            "username" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.User!.FirstName)
                : query.OrderBy(u => u.User!.FirstName),

            "trainername" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.Trainer!.ApplicationUser!.FirstName)
                : query.OrderBy(u => u.Trainer!.ApplicationUser!.FirstName),

            "membershipplanname" => filters.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(s => s.MembershipPlan!.Name)
                        : query.OrderBy(s => s.MembershipPlan!.Name),

            "startdate" => filters.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(s => s.StartDate)
                : query.OrderBy(s => s.StartDate),

            _ => query.OrderBy(u => u.User!.FirstName)
        };
        var response = sortedQuery.Select(s => new SubscriptionDto(
        s.Id,
        s.User!.Id,
        $"{s.User.FirstName} {s.User.LastName}",
        s.Trainer == null ? null : s.Trainer.UserId,
        s.Trainer == null ? null : $"{s.Trainer.ApplicationUser!.FirstName} {s.Trainer.ApplicationUser.LastName}",
        s.MembershipPlan!.Id,
        s.MembershipPlan.Name,
        s.StartDate,
        s.EndDate,
        s.Status
    ));
        var result = await PaginatedList<SubscriptionDto>.CreateAsync(response, filters.PageNumber, filters.PageSize, cancellationToken);

        return Result.Success(result);
    }
    public async Task<Result<PaginatedList<SubscriptionDto>>> GetActiveSubscriptionsAsync(RequestFilters filters, CancellationToken cancellationToken)
    {
        var query = _context.Subscriptions
           .Where(s => s.Status == SubscriptionStatus.Active &&
               (string.IsNullOrEmpty(filters.SearchValue) ||
               s.User!.FirstName.Contains(filters.SearchValue) ||
               s.User!.LastName.Contains(filters.SearchValue) ||
               s.Trainer!.ApplicationUser!.FirstName.Contains(filters.SearchValue) ||
               s.Trainer.ApplicationUser!.LastName.Contains(filters.SearchValue) ||
               s.MembershipPlan!.Name.Contains(filters.SearchValue)));


        var sortedQuery = filters.SortColumn?.ToLower() switch
        {
            "username" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.User!.FirstName)
                : query.OrderBy(u => u.User!.FirstName),

            "trainername" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.Trainer!.ApplicationUser!.FirstName)
                : query.OrderBy(u => u.Trainer!.ApplicationUser!.FirstName),

            "membershipplanname" => filters.SortDirection?.ToLower() == "desc"
                        ? query.OrderByDescending(s => s.MembershipPlan!.Name)
                        : query.OrderBy(s => s.MembershipPlan!.Name),

            "startdate" => filters.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(s => s.StartDate)
                : query.OrderBy(s => s.StartDate),

            _ => query.OrderBy(u => u.User!.FirstName)
        };
        var response = sortedQuery.Select(s => new SubscriptionDto(
        s.Id,
        s.User!.Id,
        $"{s.User.FirstName} {s.User.LastName}",
        s.Trainer == null ? null : s.Trainer.UserId,
        s.Trainer == null ? null : $"{s.Trainer.ApplicationUser!.FirstName} {s.Trainer.ApplicationUser.LastName}",
        s.MembershipPlan!.Id,
        s.MembershipPlan.Name,
        s.StartDate,
        s.EndDate,
        s.Status
    ));
        var result = await PaginatedList<SubscriptionDto>.CreateAsync(response, filters.PageNumber, filters.PageSize, cancellationToken);

        return Result.Success(result);
    }

    public async Task<Result<UserSubscriptionDto>> GetMySubscriptionAsync(string userId, CancellationToken cancellationToken)
    {
        var response = await _context.Subscriptions
            .Where(s => s.UserId == userId && s.Status == SubscriptionStatus.Active)
            .Select(s => new UserSubscriptionDto(
                s.Trainer == null ? null : $"{s.Trainer.ApplicationUser!.FirstName} {s.Trainer.ApplicationUser.LastName}",
                s.MembershipPlan!.Name,
                s.StartDate,
                s.EndDate,
                s.Status
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return Result.Failure<UserSubscriptionDto>(SubscriptionErrors.SubscriptionNotFound);

        return Result.Success(response);
    }

    public async Task<Result<SubscriptionDto>> GetSubscriptionByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _context.Subscriptions
    .Where(s => s.Id == id)
    .Select(s => new SubscriptionDto(
        s.Id,
        s.User!.Id,
        $"{s.User.FirstName} {s.User.LastName}",
        s.Trainer == null ? null : s.Trainer.UserId,
        s.Trainer == null ? null : $"{s.Trainer.ApplicationUser!.FirstName} {s.Trainer.ApplicationUser.LastName}",
        s.MembershipPlan!.Id,
        s.MembershipPlan.Name,
        s.StartDate,
        s.EndDate,
        s.Status
    ))
    .AsNoTracking()
    .FirstOrDefaultAsync(cancellationToken);
        if (response is null)
            return Result.Failure<SubscriptionDto>(SubscriptionErrors.SubscriptionNotFound);

        return Result.Success(response);
    }
}
