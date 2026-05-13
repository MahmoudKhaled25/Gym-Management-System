using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using GymManagementSystem.Contracts.WorkoutPlan;
using GymManagementSystem.Errors;

namespace GymManagementSystem.Services;

public class WorkoutPlanService(ApplicationDbContext context,UserManager<ApplicationUser> userManager) : IWorkoutPlanService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<IEnumerable<WorkoutPlanResponse>>> GetAllAsync(
     string? trainerId,
     CancellationToken cancellationToken)
    {
        var workoutPlans = await _context.WorkoutPlans
            .Where(x => trainerId == null || x.TrainerId == trainerId)
            .Select(x => new WorkoutPlanResponse(
                x.Id,
                x.Name,
                x.Description,
                x.Trainer == null ? null : $"{x.Trainer.ApplicationUser!.FirstName} {x.Trainer.ApplicationUser.LastName}",
                $"{x.User!.FirstName} {x.User.LastName}"
            ))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(workoutPlans.AsEnumerable());
    }
    public async Task<Result<IEnumerable<WorkoutPlanGroupedResponse>>> GetMemberWorkoutPlanAsync(string memberId, CancellationToken cancellationToken = default)
    {
        var result = await _context.WorkoutPlans
            .Where(x => x.UserId == memberId)
            .Select(x => new WorkoutPlanResponse(
                x.Id,
                x.Name,
                x.Description,
                x.Trainer == null ? null : $"{x.Trainer.ApplicationUser!.FirstName} {x.Trainer.ApplicationUser.LastName}",
                $"{x.User!.FirstName} {x.User.LastName}"
            ))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var grouped = result
                .GroupBy(x => x.TrainerName)
                .Select(g => new WorkoutPlanGroupedResponse(g.Key, g.ToList()));

        return Result.Success(grouped);
    }
    public async Task<Result> AddAsync(WorkoutPlanRequest request, CancellationToken cancellationToken = default)
    {
        var memberExists = await _context.Users
            .AnyAsync(x => x.Id == request.MemberId, cancellationToken);
        if (!memberExists)
            return Result.Failure(UserErrors.UserNotFound);

        if (request.TrainerId is not null)
        {
            var trainerExists = await _context.Trainers
                .AnyAsync(x => x.UserId == request.TrainerId, cancellationToken);
            if (!trainerExists)
                return Result.Failure(TrainerErrors.TrainerNotFound);
        }

        var workoutPlan = request.Adapt<WorkoutPlan>();
        _context.WorkoutPlans.Add(workoutPlan);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

 
}
