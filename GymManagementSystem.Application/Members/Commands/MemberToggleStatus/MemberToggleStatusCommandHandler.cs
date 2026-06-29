using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Consts;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Commands.MemberToggleStatus;

public class MemberToggleStatusCommandHandler(IIdentityService identityService) : IRequestHandler<MemberToggleStatusCommand, Result>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(MemberToggleStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetByIdAsync(request.MemberId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        if (!await _identityService.IsInRoleAsync(user, DefaultRoles.Member.Name))
            return Result.Failure(UserErrors.UserNotFound);

        user.LockoutEnd = user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow
            ? DateTimeOffset.UtcNow.AddYears(100)
            : null;

        var result = await _identityService.UpdateAsync(user);
        return result.Succeeded ? Result.Success() : Result.Failure(UserErrors.UpdateFailed);
    }
}
