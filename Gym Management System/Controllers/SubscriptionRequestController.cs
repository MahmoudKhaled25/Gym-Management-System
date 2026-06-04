using GymManagementSystem.Contracts.Common;
using GymManagementSystem.Contracts.SubscriptionRequest;
using GymManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SubscriptionRequestController(ISubscriptionRequestService subscriptionRequestService) : ControllerBase
{
    private readonly ISubscriptionRequestService _subscriptionRequestService = subscriptionRequestService;

    [HttpPost("")]
    [Authorize(Roles = DefaultRoles.Member.Name)]
    public async Task<IActionResult> SendSubscriptionRequest(SubscriptionRequestSendRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _subscriptionRequestService.SendRequestAsync(userId!, request.MembershipPlanId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }


    [HttpGet("")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> GetAllRequests([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _subscriptionRequestService.GetAllRequestsAsync(filters, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
