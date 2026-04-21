using Gym_Management_System.Contracts.Subscription;
using Gym_Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService = subscriptionService;


    [HttpGet("")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _subscriptionService.GetAllAsync(cancellationToken);
        return Ok(response.Value);
    }

    [HttpGet("active")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> GetAllActive(CancellationToken cancellationToken)
    {
        var response = await _subscriptionService.GetAllActiveAsync(cancellationToken);
        return Ok(response.Value);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
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
    [Authorize(Roles = DefaultRoles.Member.Name)]
    public async Task<IActionResult> Add(SubscriptionRequest request,CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _subscriptionService.AddAsync(userId!,request , cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
}
