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
  public async Task<IActionResult> GetAllTrainers()
    {
        var result = await _trainerService.GetAllTrainers();
        return Ok(result.Value);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddTrainer([FromBody] AddTrainerRequest request)
    {
        var result = await _trainerService.AddTrainerAsync(request);
        return result.IsSuccess ? Created(): result.ToProblem();
    }
}
