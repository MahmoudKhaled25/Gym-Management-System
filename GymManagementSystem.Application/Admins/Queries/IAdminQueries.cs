using GymManagementSystem.Application.Admins.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Queries;

public interface IAdminQueries
{
    Task<Result<IEnumerable<AdminDto>>> GetAllAdminsAsync(CancellationToken cancellationToken = default);

    Task<Result<DashboardDataDto>> GetDashboardAsync(CancellationToken cancellationToken = default);
}
