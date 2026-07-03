using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.SubscriptionRequests.Dtos;
using GymManagementSystem.Application.SubscriptionRequests.Queries;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Infrastructure.Queries;

public class SubscriptionRequestQueries(ApplicationDbContext context) : ISubscriptionRequestQueries
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PaginatedList<SubscriptionRequestDto>>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken)
    {
        var query = _context.SubscriptionRequests
             .Where(r => string.IsNullOrEmpty(filters.SearchValue) ||
                         (r.User!.FirstName + " " + r.User.LastName).Contains(filters.SearchValue) ||
                         r.MembershipPlan!.Name.Contains(filters.SearchValue));

        var sortedQuery = filters.SortColumn?.ToLower() switch
        {
            "membername" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.User!.FirstName)
                : query.OrderBy(r => r.User!.FirstName),

            "status" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.Status)
                : query.OrderBy(r => r.Status),

            "requestedat" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.RequestedAt)
                : query.OrderBy(r => r.RequestedAt),

            "price" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.MembershipPlan!.Price)
                : query.OrderBy(r => r.MembershipPlan!.Price),

            "membershipplanname" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(r => r.MembershipPlan!.Name)
                : query.OrderBy(r => r.MembershipPlan!.Name),

            _ => query.OrderBy(r => r.RequestedAt)
        };

        var finalQuery = sortedQuery.Select(x => new SubscriptionRequestDto(
            x.Id,
            x.User!.FirstName + " " + x.User.LastName,
            x.MembershipPlan!.Name,
            x.MembershipPlan.Price,
            x.Status,
            x.RequestedAt
        ));

        var result = await PaginatedList<SubscriptionRequestDto>.CreateAsync(
            finalQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

        return Result.Success(result);
    }
}
