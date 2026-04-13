namespace Gym_Management_System.Entities;

public class ApplicationUser : IdentityUser
{
   
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public float Weight { get; set; }
    public float Height { get; set; }
    public DateTime CreatedAt { get; set; } 

    // Navigation Properties

    public Trainer? Trainer { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<ProgressLog> ProgressLogs { get; set; } = [];
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = [];
    public List<RefreshToken> RefreshTokens { get; set; } = [];

}