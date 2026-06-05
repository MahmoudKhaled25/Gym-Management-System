namespace GymManagementSystem.Contracts.Trainer;

public record TrainerMembersResponse(
    string MemberId,
    string FullName,
    string? PhoneNumber,
    string MembershipPlanName,
    DateOnly SubscriptionEndDate
);