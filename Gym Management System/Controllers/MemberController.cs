using Gym_Management_System.Abstractions;
using Gym_Management_System.Abstractions.Consts;
using Gym_Management_System.Contracts.Member;
using Gym_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = DefaultRoles.Admin.Name)]
public class MemberController(IMemberService memberService) : ControllerBase
{
    private readonly IMemberService _memberService = memberService;

    [HttpGet("")]
   public async Task<IActionResult> GetAllMembers()
    {
        var result = await _memberService.GetAllMembersAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("active-members")]
    public async Task<IActionResult> GetActiveMembers()
    {
        var result = await _memberService.GetActiveMembersAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMemberById([FromRoute] string id,CancellationToken cancellationToken)
    {
        var result = await _memberService.GetMembersAsync(id,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> AddMember([FromBody] AddMemberRequest request, CancellationToken cancellationToken)
    {
        var result = await _memberService.AddMemberAsync(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetMemberById), new { id = result.Value!.Id }, result.Value) : result.ToProblem();
    }
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleMemberStatus([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await _memberService.ToggleStatusAsync(id, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
