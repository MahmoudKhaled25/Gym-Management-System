using GymManagementSystem.Application.Auth.Abstractions;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.ResendConfirmationEmail;

public class ResendConfirmationEmailCommandHandler(IIdentityService identityService,ILogger<ResendConfirmationEmailCommandHandler> logger,IEmailService emailService) : IRequestHandler<ResendConfirmationEmailCommand, Result>
{
    private readonly IIdentityService _identityService = identityService;
    private readonly ILogger<ResendConfirmationEmailCommandHandler> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        if (await _identityService.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        if (user.EmailConfirmed)
        {
            return Result.Failure(UserErrors.DuplicatedConfirmation);
        }

        var token = await _identityService.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        _logger.LogInformation("Confirmation Code : {code}", encodedToken);
        var confirmationLink =
                         $"https://localhost:7094/api/auth/confirm-email" +
                         $"?email={Uri.EscapeDataString(user.Email!)}" +
                         $"&token={encodedToken}";
        await _emailService.SendConfirmationEmailAsync(user, confirmationLink);
        return Result.Success();
    }
}
