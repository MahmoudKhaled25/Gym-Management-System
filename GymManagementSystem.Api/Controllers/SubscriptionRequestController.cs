using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.SubscriptionRequests.Commands.ApproveSubscriptionRequest;
using GymManagementSystem.Application.SubscriptionRequests.Commands.RejectSubscriptionRequest;
using GymManagementSystem.Application.SubscriptionRequests.Commands.SendSubscriptionRequest;
using GymManagementSystem.Application.SubscriptionRequests.Dtos;
using GymManagementSystem.Application.SubscriptionRequests.Queries.GetAllSubscriptionRequests;
using GymManagementSystem.Application.SubscriptionRequests.Queries.GetMySubscriptionRequests;
using GymManagementSystem.Domain.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SubscriptionRequestController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpPost("")]
    [Authorize(Roles = DefaultRoles.Member.Name)]
    public async Task<IActionResult> SendSubscriptionRequest([FromBody]SendSubscriptionRequestDto request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new SendSubscriptionRequestCommand(request.MembershipPlanId,userId!), cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("me")]
    [Authorize(Roles = DefaultRoles.Member.Name)]
    public async Task<IActionResult> GetMyRequests(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new GetMySubscriptionRequestsQuery(userId!), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> GetAllRequests([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllSubscriptionRequestsQuery(filters), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("{requestId}/approve")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> ApproveRequest([FromRoute]int requestId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ApproveSubscriptionRequestCommand(requestId), cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{requestId}/reject")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> RejectRequest(int requestId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RejectSubscriptionRequestCommand(requestId), cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
