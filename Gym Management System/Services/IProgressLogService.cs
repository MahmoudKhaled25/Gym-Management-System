using GymManagementSystem.Contracts.ProgressLog;

namespace GymManagementSystem.Services;

public interface IProgressLogService
{
    Task<Result<IEnumerable<ProgressLogResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result> AddAsync(string userId, ProgressLogRequest request, CancellationToken cancellationToken = default);
}
