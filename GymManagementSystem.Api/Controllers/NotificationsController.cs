using GymManagementSystem.Application.Notifications.Commands;
using GymManagementSystem.Application.Notifications.Interfaces;
using GymManagementSystem.Domain.Consts;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController(IBackgroundJobClient backgroundJobClient) : ControllerBase
{
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;

    [HttpPost("send-offer")]
    [Authorize(Roles = $"{DefaultRoles.Admin.Name},{DefaultRoles.SuperAdmin.Name}")]
    public async Task<IActionResult> SendOffer([FromBody] SendOffersCommand request)
    {
        _backgroundJobClient.Enqueue<INotificationJobService>(
            x => x.SendOfferToAllMembersAsync(request.Message));

        return Accepted("Offer is being sent to all members");
    }
}




