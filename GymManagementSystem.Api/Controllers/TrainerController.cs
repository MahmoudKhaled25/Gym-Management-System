using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Trainers.Commands.AddTrainer;
using GymManagementSystem.Application.Trainers.Commands.ToggleStatus;
using GymManagementSystem.Application.Trainers.Commands.UpdateTrainer;
using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Application.Trainers.Queries.GetActiveTrainers;
using GymManagementSystem.Application.Trainers.Queries.GetAllTrainers;
using GymManagementSystem.Application.Trainers.Queries.GetTrainerById;
using GymManagementSystem.Application.Trainers.Queries.GetTrainerMembers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GymManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

[EnableRateLimiting("General")]
public class TrainerController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters requestFilters, CancellationToken cancellationToken)
{
    var result = await _mediator.Send(new GetAllTrainersQuery(requestFilters));
    return Ok(result.Value);
}

    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpGet("active-trainers")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _mediator.Send(new GetActiveTrainersQuery());
        return Ok(result.Value);
    }
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetTrainerByIdQuery(id));
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] AddTrainerCommand request)
    {
        var result = await _mediator.Send(request);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { Id = result.Value!.Id }, result.Value) : result.ToProblem();
    }
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateTrainerRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new UpdateTrainerCommand(id,request.FirstName,request.LastName,request.Specialization));
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new TrainerToggleStatusCommand(id));
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    //    [HttpGet("my-members")]
    //    [Authorize(Roles = DefaultRoles.Trainer.Name)]
    //    public async Task<IActionResult> GetMyMembers(CancellationToken cancellationToken)
    //    {
    //        var trainerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    //        var result = await _trainerService.GetTrainerMembersAsync(trainerId, cancellationToken);
    //        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    //    }

    [HttpGet("{trainerId}/members")]
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetTrainerMembers(
        [FromRoute] string trainerId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTrainerMembersQuery(trainerId));
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
