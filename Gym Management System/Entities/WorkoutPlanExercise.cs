namespace Gym_Management_System.Entities;

public sealed class WorkoutPlanExercise
{
    public int Id { get; set; }
    
    public int Sets { get; set; }
    
    public int Reps { get; set; }
    
    public float Weight { get; set; }

    public float RestTime { get; set; } // in seconds


    public WorkoutPlan? WorkoutPlan { get; set; }
    public int WorkoutPlanId { get; set; }
    public Exercise? Exercise { get; set; }
    public int ExerciseId { get; set; }

}
