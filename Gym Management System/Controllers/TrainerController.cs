using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Trainer;
using Gym_Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = DefaultRoles.Admin.Name)]
public class TrainerController(ITrainerService trainerService) : ControllerBase
{
    private readonly ITrainerService _trainerService = trainerService;

    [HttpGet("")]
  public async Task<IActionResult> GetAll()
    {
        var result = await _trainerService.GetAllTrainersAsync();
        return Ok(result.Value);
    }

    [HttpGet("active-trainers")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _trainerService.GetActiveTrainersAsync();
        return Ok(result.Value);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]string id, CancellationToken cancellationToken = default)
    {
        var result = await _trainerService.GetTrainerByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] AddTrainerRequest request)
    {
        var result = await _trainerService.AddTrainerAsync(request);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new {Id = result.Value!.Id}, result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute]string id,[FromBody] UpdateTrainerRequest request,CancellationToken cancellationToken = default)
    {
        var result = await _trainerService.UpdateTrainerAsync(id,request,cancellationToken);
        return result.IsSuccess ? NoContent(): result.ToProblem();
    }
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var result = await _trainerService.ToggleStatusAsync(id, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
