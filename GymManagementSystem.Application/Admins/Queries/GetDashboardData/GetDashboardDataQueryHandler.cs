using GymManagementSystem.Application.Admins.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Queries.GetDashboardData;

public class GetDashboardDataQueryHandler(IAdminQueries adminQueries) : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataDto>>
{
    private readonly IAdminQueries _adminQueries = adminQueries;

    public async Task<Result<DashboardDataDto>> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken) => await _adminQueries.GetDashboardAsync(cancellationToken);
}
