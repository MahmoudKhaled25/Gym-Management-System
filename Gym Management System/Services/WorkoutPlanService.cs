using Gym_Management_System.Persistence;
using GymManagementSystem.Contracts.WorkoutPlan;

namespace GymManagementSystem.Services;

public class WorkoutPlanService(ApplicationDbContext context,UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager) : IWorkoutPlanService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

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
}
