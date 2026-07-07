using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.ProgressLogs.Commands.Add_ProgressLog;
using GymManagementSystem.Application.ProgressLogs.Commands.Delete_ProgressLog;
using GymManagementSystem.Application.ProgressLogs.Commands.Update_ProgressLog;
using GymManagementSystem.Application.ProgressLogs.Dtos;
using GymManagementSystem.Application.ProgressLogs.Queries.Get_All_ProgressLogs;
using GymManagementSystem.Application.ProgressLogs.Queries.Get_My_ProgressLogs;
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
public class ProgressLogController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters)
    {
        var result = await _mediator.Send(new GetAllProgressLogsQuery(filters));
        return Ok(result.Value);
    }

    [HttpGet("me")]
    [Authorize(Roles = $"{DefaultRoles.Member.Name}")]
    public async Task<IActionResult> GetMyProgressLogs([FromQuery]RequestFilters filters,CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new GetMyProgressLogQuery(userId!,filters));
        return Ok(result.Value);
    }

    [HttpPost("")]
    [Authorize(Roles = $"{DefaultRoles.Member.Name}")]
    public async Task<IActionResult> Add([FromBody] AddProgressLogRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new AddProgressLogCommand(userId!, request.Weight,request.Notes,request.LogDate));
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPut("{progressLogId}")]
    [Authorize(Roles = $"{DefaultRoles.Member.Name}")]
    public async Task<IActionResult> Update([FromRoute] int progressLogId, [FromBody] AddProgressLogRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new UpdateProgressLogCommand(progressLogId,userId!,request.Weight,request.Notes,request.LogDate));
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{progressLogId}")]
    [Authorize(Roles = $"{DefaultRoles.Member.Name}")]
    public async Task<IActionResult> Delete([FromRoute] int progressLogId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new DeleteProgressLogCommand(progressLogId,userId!));
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}