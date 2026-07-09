using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.ForgetPassword;

public record ForgetPasswordCommand(string Email) : IRequest<Result>;
