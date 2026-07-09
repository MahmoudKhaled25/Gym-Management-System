using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.ConfirmEmail;

public record ConfirmEmailCommand(string Email,
    string Token) : IRequest<Result>;
