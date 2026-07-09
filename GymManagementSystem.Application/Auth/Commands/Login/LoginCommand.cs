using GymManagementSystem.Application.Auth.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.Login;

public record LoginCommand(
string Email,
string Password
) : IRequest<Result<AuthDto>>;
