using GymManagementSystem.Application.Auth.Abstractions;
using GymManagementSystem.Application.Auth.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.Login;

public class LoginCommandHandler(ILogger<LoginCommandHandler> logger,IJwtProvider jwtProvider,IIdentityService identityService,IRefreshTokenService refreshTokenService) : IRequestHandler<LoginCommand, Result<AuthDto>>
{
    private readonly ILogger<LoginCommandHandler> _logger = logger;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IIdentityService _identityService = identityService;
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

    public async Task<Result<AuthDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt received");
        // check if the email is correct
        if (await _identityService.FindByEmailAsync(request.Email) is not { } user)
        {
            _logger.LogWarning("Failed login attempt for {Email}", request.Email);
            return Result.Failure<AuthDto>(UserErrors.InvalidCredentials);

        }
        // check if the password is correct


        var result = await _identityService.PasswordSignInAsync(user, request.Password, true);


        if (result.Succeeded)
        {
            var roles = await _identityService.GetRolesAsync(user.Id);
            var (token, expiresIn) = _jwtProvider.GenerateToken(user, roles);
            var refreshToken = _refreshTokenService.Generate();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });
          var updateResult =  await _identityService.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("Failed to save refresh token for user {UserId}", user.Id);
                return Result.Failure<AuthDto>(UserErrors.UpdateFailed);
            }

            var response = new AuthDto(user.Id, user.Email!, user.FirstName, user.LastName, token, expiresIn, roles, refreshToken, refreshTokenExpiration);
            _logger.LogInformation("User {UserId} logged in successfully", user.Id);
            return Result.Success(response);
        }

        var error = result.IsLockedOut ? UserErrors.LockedUser : !user.EmailConfirmed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials;

        if (error == UserErrors.LockedUser)
            _logger.LogWarning("User {Email} is locked out", request.Email);
        if (error == UserErrors.InvalidCredentials)
            _logger.LogWarning("Invalid login attempt for {Email}", request.Email);

        return Result.Failure<AuthDto>(error);
    }
}
