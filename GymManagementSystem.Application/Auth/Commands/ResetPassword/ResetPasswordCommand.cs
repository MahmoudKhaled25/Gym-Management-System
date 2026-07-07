using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.ResetPassword;

public record ResetPasswordCommand(string Email,
    string Code,
    string NewPassword) : IRequest<Result>;

