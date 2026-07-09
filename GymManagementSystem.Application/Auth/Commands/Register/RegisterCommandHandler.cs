using GymManagementSystem.Application.Auth.Abstractions;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.Register;

public class RegisterCommandHandler(IIdentityService identityService,ILogger<RegisterCommandHandler> logger,IEmailService emailService) : IRequestHandler<RegisterCommand, Result>
{
    private readonly IIdentityService _identityService = identityService;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.Email;

        var result = await _identityService.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var token = await _identityService.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            _logger.LogInformation("Confirmation Code : {code}", encodedToken);
            var confirmationLink =
                        $"https://localhost:7088/api/auth/confirm-email" +
                        $"?email={Uri.EscapeDataString(user.Email!)}" +
                        $"&token={encodedToken}";
            await _emailService.SendConfirmationEmailAsync(user, confirmationLink);

            return Result.Success();

        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
}
