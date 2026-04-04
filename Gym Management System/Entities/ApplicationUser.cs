namespace Gym_Management_System.Entities;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Id = Guid.CreateVersion7().ToString();
        SecurityStamp = Guid.CreateVersion7().ToString();
    }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public float Weight { get; set; }
    public float Height { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties

    public Trainer? Trainer { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<ProgressLog> ProgressLogs { get; set; } = [];
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = [];

}