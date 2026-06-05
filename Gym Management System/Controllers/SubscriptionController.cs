using Gym_Management_System.Contracts.Subscription;
using Gym_Management_System.Services;
using GymManagementSystem.Contracts.Common;
using GymManagementSystem.Contracts.Subscription;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace Gym_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("General")]
public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService = subscriptionService;


    [HttpGet("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var response = await _subscriptionService.GetAllAsync(filters, cancellationToken);
        return Ok(response.Value);
    }

    [HttpGet("active")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetAllActive(CancellationToken cancellationToken)
    {
        var response = await _subscriptionService.GetAllActiveAsync(cancellationToken);
        return Ok(response.Value);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetById([FromRoute]int id,CancellationToken cancellationToken)
    {
        var result = await _subscriptionService.GetByIdAsync(id,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("me")]
    [Authorize(Roles = DefaultRoles.Member.Name)]
    public async Task<IActionResult> GetMySubscription(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _subscriptionService.GetMySubscriptionAsync(userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> Add([FromBody] SubscriptionSendRequest request, CancellationToken cancellationToken)
    {
        var result = await _subscriptionService.AddAsync(request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPut("{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> Cancel([FromRoute]int id, CancellationToken cancellationToken)
    {
        var result = await _subscriptionService.CancelAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("{subscriptionId}/change-trainer")]
    [Authorize(Roles = DefaultRoles.Member.Name)]
    public async Task<IActionResult> ChangeTrainer([FromRoute] int subscriptionId, [FromBody] ChangeTrainerRequest request, CancellationToken cancellationToken)
    {
        var result = await _subscriptionService.ChangeTrainerAsync(subscriptionId, request.TrainerId, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}