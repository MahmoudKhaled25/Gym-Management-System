using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IIdentityService identityService) : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (await _identityService.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure(UserErrors.InvalidCredentials);

        var isValid = await _identityService.VerifyUserTokenAsync(user, "Email", "ResetPasswordPurpose", request.Code);
        if (!isValid)
            return Result.Failure(UserErrors.InvalidCode);

        var resetToken = await _identityService.GeneratePasswordResetTokenAsync(user);
        var result = await _identityService.ResetPasswordAsync(user, resetToken, request.NewPassword);
        if (result.Succeeded)
            return Result.Success();

        return Result.Failure(UserErrors.PasswordResetFailed);
    }
}
