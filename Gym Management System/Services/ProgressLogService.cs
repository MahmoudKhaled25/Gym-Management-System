using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using GymManagementSystem.Contracts.ProgressLog;

namespace GymManagementSystem.Services;

public class ProgressLogService(ApplicationDbContext context,UserManager<ApplicationUser> userManager) : IProgressLogService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

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
    public async Task<Result> AddAsync(string userId, ProgressLogRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user == null) 
            return Result.Failure(UserErrors.UserNotFound);

        var progressLog = request.Adapt<ProgressLog>();
        progressLog.UserId = userId;
        _context.ProgressLogs.Add(progressLog);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();    
    }

}
