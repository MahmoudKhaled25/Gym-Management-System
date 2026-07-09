using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.ProgressLogs.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Queries.Get_All_ProgressLogs;

public class GetAllProgressLogsQueryHandler(IProgressLogQueries progressLogQueries) : IRequestHandler<GetAllProgressLogsQuery, Result<PaginatedList<AllProgressLogsDto>>>
{
    private readonly IProgressLogQueries _progressLogQueries = progressLogQueries;

    public async Task<Result<PaginatedList<AllProgressLogsDto>>> Handle(GetAllProgressLogsQuery request, CancellationToken cancellationToken)
    {
        var progressLogs = await _progressLogQueries.GetAllAsync(request.Filters, cancellationToken);
        return Result.Success(progressLogs);
    }
}
