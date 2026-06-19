using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Commands.Delete_ProgressLog;

public record DeleteProgressLogCommand(int ProgressLogId, string UserId) : IRequest<Result>;

