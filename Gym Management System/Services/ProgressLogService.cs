using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using GymManagementSystem.Abstractions;
using GymManagementSystem.Contracts.Common;
using GymManagementSystem.Contracts.ProgressLog;
using GymManagementSystem.Errors;

namespace GymManagementSystem.Services;

public class ProgressLogService(ApplicationDbContext context,UserManager<ApplicationUser> userManager) : IProgressLogService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<PaginatedList<AllProgressLogsResponse>>> GetAllAsync(RequestFilters filters,CancellationToken cancellationToken = default)
    {
        var query = _context.ProgressLogs
            .Where(x =>
                   string.IsNullOrWhiteSpace(filters.SearchValue) ||

                (x.User != null &&
                 x.User.FirstName.Contains(filters.SearchValue)) ||

                (x.User != null &&
                 x.User.LastName.Contains(filters.SearchValue)) ||

                x.Notes.Contains(filters.SearchValue) ||

                x.Weight.ToString().Contains(filters.SearchValue));

        query = filters.SortColumn?.ToLower() switch
        {
            "memberfullname" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.User!.FirstName)
                       .ThenBy(x => x.User!.LastName)
                : query.OrderByDescending(x => x.User!.FirstName)
                       .ThenByDescending(x => x.User!.LastName),

            "weight" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.Weight)
                : query.OrderByDescending(x => x.Weight),

            "logdate" => filters.SortDirection?.ToLower() == "asc"
                ? query.OrderBy(x => x.LogDate)
                : query.OrderByDescending(x => x.LogDate),

            _ => query.OrderByDescending(x => x.LogDate)
        };

        var finalQuery = query.Select(x => new AllProgressLogsResponse(
            x.Id,
            $"{x.User!.FirstName} {x.User.LastName}",
            x.Weight,
            x.Notes,
            x.LogDate
        ));

        var result = await PaginatedList<AllProgressLogsResponse>.CreateAsync(
            finalQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

        return Result.Success(result);
    }
    public async Task<Result<ProgressLogGroupedResponse>> GetMyProgressLogsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var progressLogs = await _context.ProgressLogs.Where(x => x.UserId == userId)
          .Select(x => new AllProgressLogsResponse(
            x.Id,
            x.User!.FirstName + " " + x.User.LastName,
            x.Weight,
            x.Notes,
            x.LogDate
            )).AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!progressLogs.Any())
        {
            return Result.Failure<ProgressLogGroupedResponse>(ProgressLogErrors.ProgressLogNotFound);
        }

        var groupedLogs = new ProgressLogGroupedResponse(progressLogs.First().MemberFullName,
            progressLogs.Select(x => new ProgressLogResponse(x.Id,x.Weight,x.Notes,x.LogDate)));
        return Result.Success(groupedLogs);
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

    public async Task<Result> UpdateAsync(string userId,int progressLogId, ProgressLogRequest request, CancellationToken cancellationToken = default)
    {
        var progressLog = await _context.ProgressLogs
            .FirstOrDefaultAsync(x => x.Id == progressLogId && x.UserId == userId, cancellationToken);

        if (progressLog is null)
            return Result.Failure(ProgressLogErrors.ProgressLogNotFound);

         request.Adapt(progressLog);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string userId, int progressLogId, CancellationToken cancellationToken = default)
    {
        var progressLog = await _context.ProgressLogs
            .FirstOrDefaultAsync(x => x.Id == progressLogId && x.UserId == userId, cancellationToken);
        if (progressLog is null)
            return Result.Failure(ProgressLogErrors.ProgressLogNotFound);
        _context.ProgressLogs.Remove(progressLog);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
