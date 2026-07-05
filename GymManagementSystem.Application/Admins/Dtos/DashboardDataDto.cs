using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Dtos;

public record DashboardDataDto(
    // Cards
    int TotalMembers,
    int ActiveMembers,
    int TotalTrainers,
    int ActiveTrainers,
    int TotalSubscriptions,
    int ActiveSubscriptions,
    int PendingRequests,
    decimal TotalRevenue,

    // Charts
    IEnumerable<PlanSubscriptionsCount> SubscriptionsByPlan,
    IEnumerable<MonthlyCount> NewMembersPerMonth,
    IEnumerable<MonthlyRevenue> RevenuePerMonth
);

public record PlanSubscriptionsCount(string PlanName, int Count);
public record MonthlyCount(string Month, int Count);
public record MonthlyRevenue(string Month, decimal Revenue);
