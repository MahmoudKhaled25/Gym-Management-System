using GymManagementSystem.Contracts.Notifications;
using GymManagementSystem.Services;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController(IBackgroundJobClient backgroundJobClient) : ControllerBase
{
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;

    [HttpPost("send-offer")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public IActionResult SendOffer([FromBody] SendOfferRequest request)
    {
        _backgroundJobClient.Enqueue<INotificationJobService>(
            x => x.SendOfferToAllMembersAsync(request.Message));

        return Accepted("Offer is being sent to all members");
    }

}
