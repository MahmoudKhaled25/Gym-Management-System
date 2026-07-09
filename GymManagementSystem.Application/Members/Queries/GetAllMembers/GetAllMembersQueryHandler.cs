using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Queries.GetAllMembers;

public class GetAllMembersQueryHandler(IMemberQueries memberQueries) : IRequestHandler<GetAllMembersQuery, Result<PaginatedList<MemberSummaryDto>>>
{
    private readonly IMemberQueries _memberQueries = memberQueries;

    public async Task<Result<PaginatedList<MemberSummaryDto>>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _memberQueries.GetAllAsync(request.Filters, cancellationToken);
        return Result.Success(members);
    }
}
