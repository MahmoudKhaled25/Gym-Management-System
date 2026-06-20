using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.WorkoutPlanExercises.Commands.Add_WorkoutPlanExercise;
using GymManagementSystem.Application.WorkoutPlanExercises.Commands.Remove_Exercise_From_Plan;
using GymManagementSystem.Application.WorkoutPlanExercises.Commands.Update_WorkoutPlanExercise;
using GymManagementSystem.Application.WorkoutPlanExercises.Queries.GetWorkoutPlanExercise;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[EnableRateLimiting("General")]
public class WorkoutPlanExercisesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{workoutPlanId}/exercises")]
    //[Authorize]
    public async Task<IActionResult> GetExercisesByWorkoutPlanId([FromRoute] int workoutPlanId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWorkoutPlanExerciseQuery(workoutPlanId));
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("{workoutPlanId}/exercises")]
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> AddExerciseToWorkoutPlan([FromRoute] int workoutPlanId, [FromBody] AddWorkoutPlanExerciseCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddWorkoutPlanExerciseCommand(workoutPlanId,request.ExerciseId,request.Sets,request.Reps,request.Weight,request.RestTime));
        return result.IsSuccess ? CreatedAtAction(nameof(GetExercisesByWorkoutPlanId), new { workoutPlanId }, null) : result.ToProblem();
    }
    [HttpPut("{workoutPlanExerciseId}")]
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> UpdateExerciseInWorkoutPlan([FromRoute] int workoutPlanExerciseId, [FromBody] UpdateWorkoutPlanExerciseCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateWorkoutPlanExerciseCommand(workoutPlanExerciseId,request.Sets,request.Reps,request.Weight,request.RestTime));
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{workoutPlanExerciseId}")]
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> RemoveExerciseFromWorkoutPlan([FromRoute] int workoutPlanExerciseId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveExerciseFromPlanCommand(workoutPlanExerciseId));
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
