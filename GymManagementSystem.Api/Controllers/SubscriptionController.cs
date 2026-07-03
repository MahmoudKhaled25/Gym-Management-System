using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Subscriptions.Commands.AddSubscription;
using GymManagementSystem.Application.Subscriptions.Commands.CancelSubscription;
using GymManagementSystem.Application.Subscriptions.Commands.ChangeTrainer;
using GymManagementSystem.Application.Subscriptions.Queries.GetActiveSubscriptions;
using GymManagementSystem.Application.Subscriptions.Queries.GetAllSubscriptions;
using GymManagementSystem.Application.Subscriptions.Queries.GetMySubscription;
using GymManagementSystem.Application.Subscriptions.Queries.GetSubscriptionById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
//[EnableRateLimiting("General")]
public class SubscriptionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllSubscriptionsQuery(filters),cancellationToken);
        return Ok(response.Value);
    }

    [HttpGet("active")]
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetAllActive([FromQuery] RequestFilters filters,CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetActiveSubscriptionsQuery(filters),cancellationToken);
        return Ok(response.Value);
    }
    [HttpGet("{id}")]
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSubscriptionByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("me")]
    //[Authorize(Roles = DefaultRoles.Member.Name)]
    public async Task<IActionResult> GetMySubscription(CancellationToken cancellationToken)
    {
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = "019d7f6d-a87b-7e4b-b4d5-c111c3fde6e0";
        var result = await _mediator.Send(new GetMySubscriptionQuery(userId!), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> Add([FromBody] AddSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPut("{id}")]
    //[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> Cancel([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelSubscriptionCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("{subscriptionId}/change-trainer")]
    //[Authorize(Roles = DefaultRoles.Member.Name)]
    public async Task<IActionResult> ChangeTrainer([FromRoute] int subscriptionId, [FromBody] ChangeTrainerCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ChangeTrainerCommand(request.TrainerId,subscriptionId),cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}