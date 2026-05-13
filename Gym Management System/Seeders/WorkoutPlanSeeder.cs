using Gym_Management_System.Persistence;

namespace GymManagementSystem.Seeders;

public class WorkoutPlanSeeder
{
    public static async Task SeedWorkoutPlansAsync(ApplicationDbContext context)
    {
        if (await context.WorkoutPlans.AnyAsync())
            return;

        var workoutPlans = new List<WorkoutPlan>
    {
        new WorkoutPlan
        {
            Name = "Beginner Strength Program",
            Description = "A 3-day full body strength program for beginners",
            TrainerId = "019d9432-8dd9-73c5-b6e8-347d4ac603e4",
            UserId = "019d7f6d-a87b-7e4b-b4d5-c111c3fde6e0"
        },
        new WorkoutPlan
        {
            Name = "Weight Loss Cardio Plan",
            Description = "High intensity cardio program for weight loss",
            TrainerId = "019d9432-8dd9-73c5-b6e8-347d4ac603e4",
            UserId = "019d7f6d-a87b-7e4b-b4d5-c111c3fde6e0"
        },
        new WorkoutPlan
        {
            Name = "Muscle Building Program",
            Description = "Advanced hypertrophy program for muscle gain",
            TrainerId = "019d9442-9b5d-7ce1-993e-06049851112e",
            UserId = "019d8ea6-cbbe-7bae-8e61-691006ac9b48"
        },
        new WorkoutPlan
        {
            Name = "Flexibility & Mobility Plan",
            Description = "Daily stretching and mobility exercises",
            TrainerId = "019d9442-9b5d-7ce1-993e-06049851112e",
            UserId = "019d8ea6-cbbe-7bae-8e61-691006ac9b48"
        }
    };

        await context.WorkoutPlans.AddRangeAsync(workoutPlans);
        await context.SaveChangesAsync();
    }
}
