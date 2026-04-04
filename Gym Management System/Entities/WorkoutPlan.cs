namespace Gym_Management_System.Entities;

public sealed class WorkoutPlan
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Foreign Keys

    public Trainer? Trainer { get; set; } 
    public string TrainerId { get; set; } = string.Empty;

    public ApplicationUser? User { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = [];
}
