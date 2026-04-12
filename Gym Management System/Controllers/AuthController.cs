using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Auth;
using Gym_Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService,ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginRequest request,CancellationToken cancellationToken)
    {
        _logger.LogInformation("Logging with email: {email} and password {password}", request.Email, request.Password);
        var result = await _authService.GetTokenAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering with email: {email} and password {password}", request.Email, request.Password);
        // to do : Confirmation password 
        var result = await _authService.RegisterAsync(request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requesting password reset for email: {email}", request.Email);
        var result = await _authService.SendResetPasswordCodeAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
