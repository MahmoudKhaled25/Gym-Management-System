using GymManagementSystem.Contracts.ProgressLog;

namespace GymManagementSystem.Services;

public interface IProgressLogService
{
    Task<Result<IEnumerable<AllProgressLogsResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<ProgressLogGroupedResponse>> GetMyProgressLogsAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(string userId, ProgressLogRequest request, CancellationToken cancellationToken = default);
}
