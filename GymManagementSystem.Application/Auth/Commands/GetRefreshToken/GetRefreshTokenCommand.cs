using GymManagementSystem.Application.Auth.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.GetRefreshToken;

public record GetRefreshTokenCommand(
    string Token,
    string RefreshToken
) : IRequest<Result<AuthDto>>;
