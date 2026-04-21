using Gym_Management_System.Enums;

namespace Gym_Management_System.Contracts.Subscription;

public record UserSubscriptionResponse(
    string? TrainerName,
    string MembershipPlanName,
    DateOnly StartDate,
    DateOnly EndDate,
    SubscriptionStatus Status
);

