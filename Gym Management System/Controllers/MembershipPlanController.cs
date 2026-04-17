using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.MembershipPlan;
using Gym_Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = DefaultRoles.Admin.Name)]
public class MembershipPlanController(IMembershipPlanService membershipPlanService) : ControllerBase
{
    private readonly IMembershipPlanService _membershipPlanService = membershipPlanService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response =await _membershipPlanService.GetAllAsync(cancellationToken);
        return Ok(response.Value);
    }
    [HttpGet("active")]
    public async Task<IActionResult> GetAllActive()
    {
        var response = await _membershipPlanService.GetAllActiveAsync();
        return Ok(response.Value);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id ,CancellationToken cancellationToken)
    {
        var response = await _membershipPlanService.GetByIdAsync(id,cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] MembershipPlanRequest request, CancellationToken cancellationToken)
    {
        var response = await _membershipPlanService.AddAsync(request, cancellationToken);
        return response.IsSuccess ? CreatedAtAction(nameof(GetById), new { Id = response.Value!.Id }, response.Value) : response.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute]int id,[FromBody] MembershipPlanRequest request, CancellationToken cancellationToken)
    {
        var response = await _membershipPlanService.UpdateAsync(id,request, cancellationToken);
        return response.IsSuccess ? NoContent() : response.ToProblem();
    }
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _membershipPlanService.ToggleStatusAsync(id,cancellationToken);
        return response.IsSuccess ? Ok() : response.ToProblem();
    }
}
