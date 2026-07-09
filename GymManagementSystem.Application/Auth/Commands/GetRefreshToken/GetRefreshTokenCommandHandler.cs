using GymManagementSystem.Application.Auth.Abstractions;
using GymManagementSystem.Application.Auth.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.GetRefreshToken;

public class GetRefreshTokenCommandHandler(IJwtProvider jwtProvider,IIdentityService identityService,IRefreshTokenService refreshTokenService ) : IRequestHandler<GetRefreshTokenCommand, Result<AuthDto>>
{
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IIdentityService _identityService = identityService;
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

    public async Task<Result<AuthDto>> Handle(GetRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = _jwtProvider.ValidateToken(request.Token);

        if (userId is null)
            return Result.Failure<AuthDto>(UserErrors.InvalidJwtToken);

        var user = await _identityService.GetUserWithRefreshTokensAsync(userId);

        if (user is null)
            return Result.Failure<AuthDto>(UserErrors.UserNotFound);

        if (user.LockoutEnd is not null && user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthDto>(UserErrors.DisabledUser);

        if (user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthDto>(UserErrors.LockedUser);

        var userRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken && rt.IsActive);
        if (userRefreshToken is null)
            return Result.Failure<AuthDto>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;
        var roles = await _identityService.GetRolesAsync(user.Id);
        var (token, expiresIn) = _jwtProvider.GenerateToken(user, roles);
        var newRefreshToken = _refreshTokenService.Generate();
        var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = newRefreshTokenExpiration
        });
        var updateResult = await _identityService.UpdateAsync(user);

        if (!updateResult.Succeeded)
            return Result.Failure<AuthDto>(UserErrors.UpdateFailed);
        var response = new AuthDto(user.Id, user.Email!, user.FirstName, user.LastName, token, expiresIn, roles, newRefreshToken, newRefreshTokenExpiration);
        return Result.Success(response);
    }
}
