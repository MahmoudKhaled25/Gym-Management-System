using Gym_Management_System.Persistence;

namespace GymManagementSystem.Seeders;

public class ProgressLogSeeder
{
    public static async Task SeedProgressLogsAsync(ApplicationDbContext context)
    {
        if (await context.ProgressLogs.AnyAsync())
            return;

        var progressLogs = new List<ProgressLog>
    {
        // User 1
        new ProgressLog { UserId = "019d7f6d-a87b-7e4b-b4d5-c111c3fde6e0", Weight = 85, Notes = "Feeling great today!", LogDate = new DateOnly(2026, 1, 1) },
        new ProgressLog { UserId = "019d7f6d-a87b-7e4b-b4d5-c111c3fde6e0", Weight = 84, Notes = "Lost 1kg this week", LogDate = new DateOnly(2026, 1, 8) },
        new ProgressLog { UserId = "019d7f6d-a87b-7e4b-b4d5-c111c3fde6e0", Weight = 83, Notes = "Diet is working well", LogDate = new DateOnly(2026, 1, 15) },
        new ProgressLog { UserId = "019d7f6d-a87b-7e4b-b4d5-c111c3fde6e0", Weight = 82, Notes = "Feeling stronger", LogDate = new DateOnly(2026, 1, 22) },

        // User 2
        new ProgressLog { UserId = "019d8ea6-cbbe-7bae-8e61-691006ac9b48", Weight = 90, Notes = "Starting my journey", LogDate = new DateOnly(2026, 1, 1) },
        new ProgressLog { UserId = "019d8ea6-cbbe-7bae-8e61-691006ac9b48", Weight = 89, Notes = "Good progress this week", LogDate = new DateOnly(2026, 1, 8) },
        new ProgressLog { UserId = "019d8ea6-cbbe-7bae-8e61-691006ac9b48", Weight = 88, Notes = "Gym sessions paying off", LogDate = new DateOnly(2026, 1, 15) },
        new ProgressLog { UserId = "019d8ea6-cbbe-7bae-8e61-691006ac9b48", Weight = 87, Notes = "Feeling more energetic", LogDate = new DateOnly(2026, 1, 22) },
    };

        await context.ProgressLogs.AddRangeAsync(progressLogs);
        await context.SaveChangesAsync();
    }
}
