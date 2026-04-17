using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.MembershipPlan;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using System.Numerics;

namespace Gym_Management_System.Services;

public class MembershipPlanService(UserManager<ApplicationUser> userManager,ApplicationDbContext context) : IMembershipPlanService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<MembershipPlanResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var plans = await _context.MembershipPlans
            .ToListAsync(cancellationToken);
        var response = plans.Adapt<IEnumerable<MembershipPlanResponse>>();
        return Result.Success(response);
    }
    public async Task<Result<IEnumerable<MembershipPlanResponse>>> GetAllActiveAsync()
    {
        var activePlans =await _context.MembershipPlans.Where(x => x.IsActive).ToListAsync();
        var response = activePlans.Adapt<IEnumerable<MembershipPlanResponse>>();
        return Result.Success(response);

    }
    public async Task<Result<MembershipPlanResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var plan = await _context.MembershipPlans.FindAsync(id);
        if (plan == null)
            return Result.Failure<MembershipPlanResponse>(MembershipPlanErrors.PlanNotFound);

        var response = plan.Adapt<MembershipPlanResponse>();
        return Result.Success(response);
    }
    public async Task<Result<MembershipPlanResponse>> AddAsync(MembershipPlanRequest request,CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim().ToLower();

        var isPlanExists = await _context.MembershipPlans
            .AnyAsync(x => x.Name.ToLower() == normalizedName, cancellationToken);

        if (isPlanExists)
            return Result.Failure<MembershipPlanResponse>(MembershipPlanErrors.PlanExists);

        var plan = request.Adapt<MembershipPlan>();
        plan.Name = normalizedName;

        await _context.MembershipPlans.AddAsync(plan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = plan.Adapt<MembershipPlanResponse>();

        return Result.Success(response);
    }
    public async Task<Result> UpdateAsync(int id, MembershipPlanRequest request, CancellationToken cancellationToken)
    {
        var plan = await _context.MembershipPlans
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (plan is null)
            return Result.Failure(MembershipPlanErrors.PlanNotFound);

        var normalizedName = request.Name.Trim().ToLower();

        var isNameExists = await _context.MembershipPlans
            .AnyAsync(x => x.Id != id && x.Name.ToLower() == normalizedName, cancellationToken);

        if (isNameExists)
            return Result.Failure(MembershipPlanErrors.PlanExists);

        plan.Name = normalizedName;
        plan.Description = request.Description;
        plan.Price = request.Price;
        plan.DurationInDays = request.DurationInDays;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken)
    {
        var plan = await _context.MembershipPlans
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (plan is null)
            return Result.Failure(MembershipPlanErrors.PlanNotFound);

        plan.IsActive = !plan.IsActive;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

   
}
