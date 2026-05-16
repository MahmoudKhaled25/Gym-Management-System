using GymManagementSystem.Contracts.WorkoutPlanExercise;
using GymManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GymManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("General")]
public class WorkoutPlanExercisesController(IWorkoutPlanExerciseService workoutPlanExerciseService) : ControllerBase
{
    private readonly IWorkoutPlanExerciseService _workoutPlanExerciseService = workoutPlanExerciseService;

    [HttpGet("{workoutPlanId}/exercises")]
    [Authorize]
    public async Task<IActionResult> GetExercisesByWorkoutPlanId([FromRoute] int workoutPlanId, CancellationToken cancellationToken)
    {
        var result = await _workoutPlanExerciseService.GetWorkoutPlanExerciseAsync(workoutPlanId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("{workoutPlanId}/exercises")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name}")]
    public async Task<IActionResult> AddExerciseToWorkoutPlan([FromRoute] int workoutPlanId, [FromBody] WorkoutPlanExerciseRequest request, CancellationToken cancellationToken)
    {
        var result = await _workoutPlanExerciseService.AddExerciseToPlanAsync(workoutPlanId, request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetExercisesByWorkoutPlanId), new { workoutPlanId }, null) : result.ToProblem();
    }
    [HttpPut("{workoutPlanExerciseId}")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name}")]
    public async Task<IActionResult> UpdateExerciseInWorkoutPlan([FromRoute] int workoutPlanExerciseId ,[FromBody] UpdateWorkoutPlanExerciseRequest request, CancellationToken cancellationToken)
    {
        var result = await _workoutPlanExerciseService.UpdateExerciseInPlanAsync(workoutPlanExerciseId, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{workoutPlanExerciseId}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> RemoveExerciseFromWorkoutPlan([FromRoute] int workoutPlanExerciseId, CancellationToken cancellationToken)
    {
        var result = await _workoutPlanExerciseService.RemoveExerciseFromPlanAsync(workoutPlanExerciseId, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
