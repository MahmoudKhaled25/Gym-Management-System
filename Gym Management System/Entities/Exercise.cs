namespace Gym_Management_System.Entities;

public sealed class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string MuscleGroup { get; set; } = string.Empty;

    // Foreign Keys
    public ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = [];
}
