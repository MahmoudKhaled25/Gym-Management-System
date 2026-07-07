using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Accounts.Commands.ChangePassword;
using GymManagementSystem.Application.Accounts.Commands.DeleteProfileImage;
using GymManagementSystem.Application.Accounts.Commands.UpdateUserProfile;
using GymManagementSystem.Application.Accounts.Commands.UploadProfileImage;
using GymManagementSystem.Application.Accounts.Dtos;
using GymManagementSystem.Application.Accounts.Queries.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace GymManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Account")]
public class AccountController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new GetProfileQuery(userId!), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new UpdateUserProfileCommand(userId!,request.FirstName,request.LastName,request.DateOfBirth,request.Weight,request.Height,request.PhoneNumber), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordDto request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new ChangePasswordCommand(userId!, request.OldPassword, request.NewPassword), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPost("profile-image")]
    public async Task<IActionResult> UploadProfileImage([FromForm] UploadProfileImageRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new UploadProfileImageCommand(userId!, request.Image), cancellationToken);
        return result.IsSuccess ? Ok(new { result.Value }) : result.ToProblem();
    }

    [HttpDelete("profile-image")]
    public async Task<IActionResult> DeleteProfileImage(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new DeleteProfileImageCommand(userId!), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
