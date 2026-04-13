using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;

    [HttpGet("")]
    public async Task <IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _accountService.GetProfileAsync(userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) :result.ToProblem();
    }
    [HttpPut("")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _accountService.UpdateProfileAsync(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
