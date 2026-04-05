namespace Gym_Management_System.Entities;

public sealed class ProgressLog
{
    public int Id { get; set; }

    public float Weight { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateOnly LogDate { get; set; } 


    // Foreign Keys

    public ApplicationUser? User { get; set; } 
    public string UserId { get; set; } = string.Empty;


}
