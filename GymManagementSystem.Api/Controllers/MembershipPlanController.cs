using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.MembershipPlans.Commands.Add_Plan;
using GymManagementSystem.Application.MembershipPlans.Commands.Toggle_Status;
using GymManagementSystem.Application.MembershipPlans.Commands.Update_Plan;
using GymManagementSystem.Application.MembershipPlans.Queries.Get_Active_Plans;
using GymManagementSystem.Application.MembershipPlans.Queries.Get_All_Plans;
using GymManagementSystem.Application.MembershipPlans.Queries.Get_By_Id;
using GymManagementSystem.Domain.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GymManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
[EnableRateLimiting("General")]
public class MembershipPlanController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(new GetAllPlansQuery());
        return Ok(response.Value);
    }
    [HttpGet("active")]
    public async Task<IActionResult> GetAllActive()
    {
        var response = await _mediator.Send(new GetActivePlansQuery());
        return Ok(response.Value);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetPlanByIdQuery(id));
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] AddPlansCommand request)
    {
        var response = await _mediator.Send(request);
        return response.IsSuccess ? CreatedAtAction(nameof(GetById), new { Id = response.Value!.Id }, response.Value) : response.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePlanCommand request)
    {
        var response = await _mediator.Send(new UpdatePlanCommand(id,request.Name,request.Description,request.Price,request.DurationInDays));
        return response.IsSuccess ? NoContent() : response.ToProblem();
    }
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id)
    {
        var response = await _mediator.Send(new PlanToggleStatusCommand(id));
        return response.IsSuccess ? Ok() : response.ToProblem();
    }
}
