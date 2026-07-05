using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Commands.ChangePassword;

public record ChangePasswordCommand(string UserId, string OldPassword, string NewPassword) : IRequest<Result>;
