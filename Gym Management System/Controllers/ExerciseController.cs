using Gym_Management_System.Contracts.Exercise;
using Gym_Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciseController(IExerciseService exerciseService) : ControllerBase
{
    private readonly IExerciseService _exerciseService = exerciseService;

    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _exerciseService.GetAllAsync(cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _exerciseService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name}")]
    public async Task<IActionResult> Add([FromBody] ExerciseRequest request, CancellationToken cancellationToken)
    {
        var result = await _exerciseService.AddAsync(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value) : result.ToProblem();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.Trainer.Name}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ExerciseRequest request, CancellationToken cancellationToken)
    {
        var result = await _exerciseService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _exerciseService.ToggleStatusAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
