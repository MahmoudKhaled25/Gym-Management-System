using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Application.ProgressLogs.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Queries;

public interface IProgressLogQueries
{
   Task<PaginatedList<AllProgressLogsDto>> GetAllAsync(RequestFilters filters,CancellationToken cancellationToken = default);

    
   Task<PaginatedList<MyProgressLogsDto>> GetMyAsync(string userId,RequestFilters filters,CancellationToken cancellationToken = default);

}
