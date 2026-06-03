using GymManagementSystem.Enums;

namespace GymManagementSystem.Entities;

public class SubscriptionRequest
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser? User { get; set; }

    public int MembershipPlanId { get; set; }
    public MembershipPlan? MembershipPlan { get; set; }
    public SubscriptionRequestStatus Status { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}
