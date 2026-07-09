using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Commands.Add_ProgressLog;

public record AddProgressLogCommand(string UserId, float Weight, string Notes, DateOnly LogDate) : IRequest<Result>;
