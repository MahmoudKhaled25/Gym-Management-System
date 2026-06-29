using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Queries.GetActiveMembers;

public class GetActiveMembersQueryHandler(IMemberQueries memberQueries) : IRequestHandler<GetActiveMembersQuery, Result<PaginatedList<MemberSummaryDto>>>
{
    private readonly IMemberQueries _memberQueries = memberQueries;

    public async Task<Result<PaginatedList<MemberSummaryDto>>> Handle(GetActiveMembersQuery request, CancellationToken cancellationToken)
    {
        var activeMembers = await _memberQueries.GetActiveAsync(request.Filters, cancellationToken);
        return Result.Success(activeMembers);
    }
}
