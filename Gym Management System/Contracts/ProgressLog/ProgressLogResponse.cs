using GymManagementSystem.Contracts.WorkoutPlanExercise;

namespace GymManagementSystem.Contracts.ProgressLog;

public record AllProgressLogsResponse(int Id,string MemberName,float Weight,string Notes, DateOnly LogDate);

public record ProgressLogResponse(int Id,float Weight,string Notes, DateOnly LogDate);

public record ProgressLogGroupedResponse(
    string MemberName,
    IEnumerable<ProgressLogResponse> ProgressLogs
);