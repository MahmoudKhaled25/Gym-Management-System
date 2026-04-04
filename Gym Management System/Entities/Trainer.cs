using System.ComponentModel.DataAnnotations;

namespace Gym_Management_System.Entities;

public sealed class Trainer
{
    [Key]
    public string UserId { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public ApplicationUser? ApplicationUser { get; set; }
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = [];
}
