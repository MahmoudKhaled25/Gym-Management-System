using GymManagementSystem.Api.Extensions;
using GymManagementSystem.Application.Auth.Commands.ConfirmEmail;
using GymManagementSystem.Application.Auth.Commands.ForgetPassword;
using GymManagementSystem.Application.Auth.Commands.GetRefreshToken;
using GymManagementSystem.Application.Auth.Commands.Login;
using GymManagementSystem.Application.Auth.Commands.Register;
using GymManagementSystem.Application.Auth.Commands.ResendConfirmationEmail;
using GymManagementSystem.Application.Auth.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GymManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("AuthByIp")]
public class AuthController(ILogger<AuthController> logger,IMediator mediator) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [DisableRateLimiting()]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] GetRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Refreshing token for user with token: {token}", request.Token);
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("register")]

    public async Task<IActionResult> Register([FromBody] RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPost("forget-password")]

    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requesting password reset for email: {email}", request.Email);
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Resetting password for email: {email}", request.Email);
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Confirming email for email: {email}", request.Email);
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Resending confirmation email for email: {email}", request.Email);
        var result = await _mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

}