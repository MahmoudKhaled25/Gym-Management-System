namespace GymManagementSystem.Contracts.ProgressLog;

public record ProgressLogResponse(int Id,string MemberName,float Weight,string Notes, DateOnly LogDate);

