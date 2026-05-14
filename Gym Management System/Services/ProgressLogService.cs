using Gym_Management_System.Persistence;
using GymManagementSystem.Contracts.ProgressLog;

namespace GymManagementSystem.Services;

public class ProgressLogService(ApplicationDbContext context) : IProgressLogService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<ProgressLogResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var progressLogs = await _context.ProgressLogs.Select(x => new ProgressLogResponse(
            x.Id,
            x.User!.FirstName + " " + x.User.LastName,
            x.Weight,
            x.Notes,
            x.LogDate
            )).AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(progressLogs.AsEnumerable());

    }
}
