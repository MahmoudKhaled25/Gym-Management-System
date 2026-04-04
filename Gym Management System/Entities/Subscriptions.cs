using Gym_Management_System.Enums;

namespace Gym_Management_System.Entities;

public sealed class Subscription
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public SubscriptionStatus Status { get; set; }

    // Foreign Keys
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public int MembershipPlanId { get; set; }
    public MembershipPlan? MembershipPlan { get; set; }

    public string? TrainerId { get; set; }
    public Trainer? Trainer { get; set; }
}