using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Members.Commands.AddMember;
using GymManagementSystem.Application.Members.Commands.MemberToggleStatus;
using GymManagementSystem.Application.Members.Queries.GetActiveMembers;
using GymManagementSystem.Application.Members.Queries.GetAllMembers;
using GymManagementSystem.Application.Members.Queries.GetMemberById;
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
public class MemberController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpGet("")]
    public async Task<IActionResult> GetAllMembers([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllMembersQuery(filters),cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("active-members")]
    public async Task<IActionResult> GetActiveMembers([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveMembersQuery(filters),cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMemberById([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMemberByIdQuery(id),cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> AddMember([FromBody] AddMemberCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetMemberById), new { id = result.Value!.Id }, result.Value) : result.ToProblem();
    }
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleMemberStatus([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new MemberToggleStatusCommand(id),cancellationToken);
        return result.IsSuccess
     ? NoContent()
     : result.ToProblem();
    }
}
