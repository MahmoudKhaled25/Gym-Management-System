using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Trainer;
using Gym_Management_System.Services;
using GymManagementSystem.Contracts.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace Gym_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]

[EnableRateLimiting("General")]
public class TrainerController(ITrainerService trainerService) : ControllerBase
{
    private readonly ITrainerService _trainerService = trainerService;

    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpGet("")]
  public async Task<IActionResult> GetAll([FromQuery] RequestFilters requestFilters,CancellationToken cancellationToken)
    {
        var result = await _trainerService.GetAllTrainersAsync(requestFilters,cancellationToken);
        return Ok(result.Value);
    }

    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpGet("active-trainers")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _trainerService.GetActiveTrainersAsync();
        return Ok(result.Value);
    }
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]string id, CancellationToken cancellationToken = default)
    {
        var result = await _trainerService.GetTrainerByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] AddTrainerRequest request)
    {
        var result = await _trainerService.AddTrainerAsync(request);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new {Id = result.Value!.Id}, result.Value) : result.ToProblem();
    }
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute]string id,[FromBody] UpdateTrainerRequest request,CancellationToken cancellationToken = default)
    {
        var result = await _trainerService.UpdateTrainerAsync(id,request,cancellationToken);
        return result.IsSuccess ? NoContent(): result.ToProblem();
    }
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var result = await _trainerService.ToggleStatusAsync(id, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("my-members")]
    [Authorize(Roles = DefaultRoles.Trainer.Name)]
    public async Task<IActionResult> GetMyMembers(CancellationToken cancellationToken)
    {
        var trainerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _trainerService.GetTrainerMembersAsync(trainerId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{trainerId}/members")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetTrainerMembers(
        [FromRoute] string trainerId,
        CancellationToken cancellationToken)
    {
        var result = await _trainerService.GetTrainerMembersAsync(trainerId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
