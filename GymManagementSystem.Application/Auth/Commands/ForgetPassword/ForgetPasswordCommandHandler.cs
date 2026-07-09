using GymManagementSystem.Application.Auth.Abstractions;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.ForgetPassword;

public class ForgetPasswordCommandHandler(IIdentityService identityService,ILogger<ForgetPasswordCommandHandler> logger,IEmailService emailService) : IRequestHandler<ForgetPasswordCommand, Result>
{
    private readonly IIdentityService _identityService = identityService;
    private readonly ILogger<ForgetPasswordCommandHandler> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (await _identityService.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed with { StatusCode = StatusCodes.Status400BadRequest });

        var otpCode = await _identityService.GenerateUserTokenAsync(user, "Email", "ResetPasswordPurpose");
        _logger.LogInformation("Password reset OTP generated for user {Email}: {OtpCode}", user.Email, otpCode);
        //var resetLink =
        //                 $"https://localhost:7088/api/auth/reset-password" +
        //                 $"?email={Uri.EscapeDataString(user.Email!)}" +
        //                 $"&token={otpCode}";
        await _emailService.SendResetPasswordEmailAsync(user, otpCode);
        return Result.Success();
    }
}
