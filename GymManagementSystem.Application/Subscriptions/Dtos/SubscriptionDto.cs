using GymManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Subscriptions.Dtos;

public record UserSubscriptionDto(
    string? TrainerName,
    string MembershipPlanName,
    DateOnly StartDate,
    DateOnly EndDate,
    SubscriptionStatus Status
);

public record SubscriptionDto(int Id, string UserId, string UserName, string? TrainerId, string? TrainerName, int MembershipPlanId, string MembershipPlanName, DateOnly StartDate, DateOnly EndDate, SubscriptionStatus Status);
