using Gym_Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService = subscriptionService;


    [HttpGet("")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _subscriptionService.GetAllAsync();
        return Ok(response.Value);
    }
}
