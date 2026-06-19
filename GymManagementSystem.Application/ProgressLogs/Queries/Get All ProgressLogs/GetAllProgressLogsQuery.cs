using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.ProgressLogs.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Queries.Get_All_ProgressLogs;

public record GetAllProgressLogsQuery(RequestFilters Filters) : IRequest<Result<PaginatedList<AllProgressLogsDto>>>;
