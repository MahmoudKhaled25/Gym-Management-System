using GymManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
}
