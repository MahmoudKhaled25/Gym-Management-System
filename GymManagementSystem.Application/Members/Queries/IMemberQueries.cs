using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Members.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Queries;

public interface IMemberQueries
{
    Task<PaginatedList<MemberSummaryDto>> GetAllAsync(RequestFilters filters,CancellationToken cancellationToken);

    Task<PaginatedList<MemberSummaryDto>> GetActiveAsync(RequestFilters filters, CancellationToken cancellationToken);

    Task<MemberSummaryDto?> GetByIdAsync(string memberId, CancellationToken cancellationToken = default);
}
