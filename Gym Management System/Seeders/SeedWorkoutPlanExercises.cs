using Gym_Management_System.Persistence;

namespace GymManagementSystem.Seeders;

public class WorkoutPlanExercisesSeeder
{
    public static async Task SeedWorkoutPlanExercisesAsync(ApplicationDbContext context)
    {
        if (await context.WorkoutPlanExercises.AnyAsync())
            return;

        var workoutPlanExercises = new List<WorkoutPlanExercise>
    {
        // WorkoutPlan 1 - Beginner Strength Program
        new WorkoutPlanExercise { WorkoutPlanId = 1, ExerciseId = 1, Sets = 4, Reps = 10, Weight = 60, RestTime = 90 },
        new WorkoutPlanExercise { WorkoutPlanId = 1, ExerciseId = 2, Sets = 3, Reps = 12, Weight = 80, RestTime = 120 },
        new WorkoutPlanExercise { WorkoutPlanId = 1, ExerciseId = 8, Sets = 3, Reps = 30, Weight = 0, RestTime = 60 },

        // WorkoutPlan 2 - Weight Loss Cardio Plan
        new WorkoutPlanExercise { WorkoutPlanId = 2, ExerciseId = 3, Sets = 3, Reps = 8, Weight = 100, RestTime = 120 },
        new WorkoutPlanExercise { WorkoutPlanId = 2, ExerciseId = 4, Sets = 3, Reps = 10, Weight = 0, RestTime = 90 },
        new WorkoutPlanExercise { WorkoutPlanId = 2, ExerciseId = 8, Sets = 4, Reps = 45, Weight = 0, RestTime = 60 },

        // WorkoutPlan 3 - Muscle Building Program
        new WorkoutPlanExercise { WorkoutPlanId = 3, ExerciseId = 1, Sets = 5, Reps = 8, Weight = 80, RestTime = 120 },
        new WorkoutPlanExercise { WorkoutPlanId = 3, ExerciseId = 5, Sets = 4, Reps = 10, Weight = 50, RestTime = 90 },
        new WorkoutPlanExercise { WorkoutPlanId = 3, ExerciseId = 6, Sets = 3, Reps = 12, Weight = 20, RestTime = 60 },
        new WorkoutPlanExercise { WorkoutPlanId = 3, ExerciseId = 7, Sets = 3, Reps = 12, Weight = 0, RestTime = 60 },

        // WorkoutPlan 4 - Flexibility & Mobility Plan
        new WorkoutPlanExercise { WorkoutPlanId = 4, ExerciseId = 8, Sets = 3, Reps = 60, Weight = 0, RestTime = 30 },
        new WorkoutPlanExercise { WorkoutPlanId = 4, ExerciseId = 9, Sets = 3, Reps = 15, Weight = 100, RestTime = 90 },
        new WorkoutPlanExercise { WorkoutPlanId = 4, ExerciseId = 10, Sets = 4, Reps = 12, Weight = 50, RestTime = 60 },

        // WorkoutPlan 5
        new WorkoutPlanExercise { WorkoutPlanId = 5, ExerciseId = 2, Sets = 4, Reps = 10, Weight = 90, RestTime = 120 },
        new WorkoutPlanExercise { WorkoutPlanId = 5, ExerciseId = 3, Sets = 3, Reps = 8, Weight = 120, RestTime = 120 },
        new WorkoutPlanExercise { WorkoutPlanId = 5, ExerciseId = 4, Sets = 3, Reps = 10, Weight = 0, RestTime = 90 },
    };

        await context.WorkoutPlanExercises.AddRangeAsync(workoutPlanExercises);
        await context.SaveChangesAsync();
    }
}