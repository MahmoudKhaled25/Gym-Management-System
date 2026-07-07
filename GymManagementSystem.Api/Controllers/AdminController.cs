using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Admins.Commands.AddAdmin;
using GymManagementSystem.Application.Admins.Commands.ToggleStatus;
using GymManagementSystem.Application.Admins.Queries.GetAllAdmins;
using GymManagementSystem.Application.Admins.Queries.GetDashboardData;
using GymManagementSystem.Domain.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = DefaultRoles.SuperAdmin.Name)]
public class AdminController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    public async Task<IActionResult> GetAllAdmins(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllAdminsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> AddAdmin([FromBody] AddAdminCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AdminToggleStatusCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpGet("Dashboard")]
    public async Task<IActionResult> GetDashboardData(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDashboardDataQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
