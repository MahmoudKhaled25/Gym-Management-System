using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.WorkoutPlans.Commands.AddWorkoutPlan;
using GymManagementSystem.Application.WorkoutPlans.Commands.DeleteWorkoutPlan;
using GymManagementSystem.Application.WorkoutPlans.Commands.UpdateWorkoutPlan;
using GymManagementSystem.Application.WorkoutPlans.Queries.GetAllWorkoutPlans;
using GymManagementSystem.Application.WorkoutPlans.Queries.GetMyWorkoutPlans;
using GymManagementSystem.Domain.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace GymManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("General")]
public class WorkoutPlanController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, [FromQuery] string? trainerId, CancellationToken cancellationToken)
    {
        if (User.IsInRole(DefaultRoles.Trainer.Name))
            trainerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new GetAllWorkoutPlansQuery(filters,trainerId), cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("me")]
    [Authorize(Roles = $"{DefaultRoles.Member.Name}")]
    public async Task<IActionResult> GetMemberWorkoutPlans(CancellationToken cancellationToken)
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new GetMyWorkoutPlansQuery(memberId!), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> Add([FromBody] AddWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetAll), new { trainerId = request.TrainerId }, null) : result.ToProblem();
    }
    [HttpPut("{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateWorkoutPlanCommand(id,request.Name,request.Description,request.MemberId,request.TrainerId));
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteWorkoutPlanCommand(id));
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}