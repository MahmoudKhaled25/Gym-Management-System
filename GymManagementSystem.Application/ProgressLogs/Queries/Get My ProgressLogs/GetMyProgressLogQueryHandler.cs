using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.ProgressLogs.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Errors;
using MediatR;

namespace GymManagementSystem.Application.ProgressLogs.Queries.Get_My_ProgressLogs;

public class GetMyProgressLogQueryHandler(IProgressLogQueries progressLogQueries) : IRequestHandler<GetMyProgressLogQuery, Result<PaginatedList<MyProgressLogsDto>>>
{
    private readonly IProgressLogQueries _progressLogQueries = progressLogQueries;

    public async Task<Result<PaginatedList<MyProgressLogsDto>>> Handle(GetMyProgressLogQuery request, CancellationToken cancellationToken)
    {
        var myProgressLogs = await _progressLogQueries.GetMyAsync(request.UserId,request.Filters,cancellationToken);

        return Result.Success(myProgressLogs);
    }
}
