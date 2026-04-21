namespace Gym_Management_System.Entities;

public sealed class MembershipPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }    
    public int DurationInDays { get; set; }
    public bool IsActive { get; set; } = true;

    public int? SessionsPerMonth { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; } = [];

    
}
