using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Exercises.Commands;
using GymManagementSystem.Application.Exercises.Commands.Add_Exercise;
using GymManagementSystem.Application.Exercises.Commands.ToggleStatus;
using GymManagementSystem.Application.Exercises.Queries.Get_All_Exercises;
using GymManagementSystem.Application.Exercises.Queries.Get_Exercise_By_Id;
using GymManagementSystem.Domain.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GymManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("General")]
public class ExerciseController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllExercisesQuery(filters));
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetExerciseByIdQuery(id));
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> Add([FromBody] AddExerciseCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request);
        return result.IsSuccess
               ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
               : result.ToProblem();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateExerciseCommand request)
    {
        var result = await _mediator.Send(new UpdateExerciseCommand(id, request.Name,request.Description,request.MuscleGroup));
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id)
    {
        var result = await _mediator.Send(new ExerciseToggleStatusCommand(id));
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
