using GymManagementSystem.Enums;

namespace GymManagementSystem.Contracts.SubscriptionRequest;

public record SubscriptionRequestResponse
(   int Id,
    string MemberName,
    string MembershipPlanName,
    decimal Price,
    SubscriptionRequestStatus Status,
    DateTime RequestedAt);
