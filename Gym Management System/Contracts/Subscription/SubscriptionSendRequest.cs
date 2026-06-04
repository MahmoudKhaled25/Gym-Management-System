using Gym_Management_System.Enums;

namespace Gym_Management_System.Contracts.Subscription;

public record SubscriptionSendRequest(string UserId, int MembershipPlanId);

