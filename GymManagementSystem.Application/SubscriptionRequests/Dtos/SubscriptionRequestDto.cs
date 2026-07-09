using GymManagementSystem.Domain.Enums;

namespace GymManagementSystem.Application.SubscriptionRequests.Dtos;

public record SubscriptionRequestDto
    (int Id,
    string MemberName,
    string MembershipPlanName,
    decimal Price,
    SubscriptionRequestStatus Status,
    DateTime RequestedAt);

public record SendSubscriptionRequestDto(int MembershipPlanId);