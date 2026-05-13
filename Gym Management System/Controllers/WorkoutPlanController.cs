using GymManagementSystem.Contracts.WorkoutPlan;
using GymManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Gym_Management_System.Abstractions.Consts.DefaultRoles;

namespace GymManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkoutPlanController(IWorkoutPlanService workoutPlanService) : ControllerBase
{
    private readonly IWorkoutPlanService _workoutPlanService = workoutPlanService;

    [HttpGet("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name}")]
    public async Task<IActionResult> GetAll([FromQuery] string? trainerId, CancellationToken cancellationToken)
    {
        if (User.IsInRole(DefaultRoles.Trainer.Name))
            trainerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _workoutPlanService.GetAllAsync(trainerId, cancellationToken);
        return Ok(result.Value);
    }
    [HttpPost("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name}")]
    public async Task<IActionResult> Add([FromBody] WorkoutPlanRequest request, CancellationToken cancellationToken)
    {
        var result = await _workoutPlanService.AddAsync(request, cancellationToken);
       return result.IsSuccess ? CreatedAtAction(nameof(GetAll), new { trainerId = request.TrainerId }, null) : result.ToProblem();
    }
}