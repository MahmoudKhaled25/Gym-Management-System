using GymManagementSystem.Contracts.ProgressLog;
using GymManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProgressLogController(IProgressLogService progressLogService) : ControllerBase
{
    private readonly IProgressLogService _progressLogService = progressLogService;

    [HttpGet("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name}")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _progressLogService.GetAllAsync(cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("me")]
    [Authorize(Roles = $"{DefaultRoles.Member.Name}")]
    public async Task<IActionResult> GetMyProgressLogs(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _progressLogService.GetMyProgressLogsAsync(userId!, cancellationToken);
        return Ok(result.Value);
    }

    [HttpPost("")]
    [Authorize(Roles = $"{DefaultRoles.Member.Name}")]
    public async Task<IActionResult> Add([FromBody] ProgressLogRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _progressLogService.AddAsync(userId!,request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
}