using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Commands.Update_ProgressLog;

public record UpdateProgressLogCommand(int ProgressLogId,string UserId, float Weight, string Notes, DateOnly LogDate) : IRequest<Result>;
