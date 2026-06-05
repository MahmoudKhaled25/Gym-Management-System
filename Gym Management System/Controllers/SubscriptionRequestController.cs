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

    [HttpGet("me")]
    [Authorize(Roles = DefaultRoles.Member.Name)]
    public async Task<IActionResult> GetMyRequests(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _subscriptionRequestService.GetMyRequestsAsync(userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetAllRequests([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _subscriptionRequestService.GetAllRequestsAsync(filters, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("{requestId}/approve")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
  public async Task<IActionResult> ApproveRequest(int requestId, CancellationToken cancellationToken)
    {
        var result = await _subscriptionRequestService.ApproveRequestAsync(requestId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{requestId}/reject")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> RejectRequest(int requestId, CancellationToken cancellationToken)
    {
        var result = await _subscriptionRequestService.RejectRequestAsync(requestId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
