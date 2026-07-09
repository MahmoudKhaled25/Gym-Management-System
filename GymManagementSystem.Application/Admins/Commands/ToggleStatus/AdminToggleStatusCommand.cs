using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Commands.ToggleStatus;

public record AdminToggleStatusCommand(string AdminId) : IRequest<Result>;
