using GymManagementSystem.Application.Admins.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Consts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Queries.GetAllAdmins;

public class GetAllAdminsQueryHandler(IAdminQueries adminQueries) : IRequestHandler<GetAllAdminsQuery, Result<IEnumerable<AdminDto>>>
{
    private readonly IAdminQueries _adminQueries = adminQueries;

    public async Task<Result<IEnumerable<AdminDto>>> Handle(GetAllAdminsQuery request, CancellationToken cancellationToken) =>
     await _adminQueries.GetAllAdminsAsync(cancellationToken);
    
}
