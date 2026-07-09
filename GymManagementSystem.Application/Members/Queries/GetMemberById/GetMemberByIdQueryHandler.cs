using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Queries.GetMemberById;

public class GetMemberByIdQueryHandler(IMemberQueries memberQueries) : IRequestHandler<GetMemberByIdQuery, Result<MemberSummaryDto>>
{
    private readonly IMemberQueries _memberQueries = memberQueries;

    public async Task<Result<MemberSummaryDto>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _memberQueries.GetByIdAsync(request.MemberId, cancellationToken);
        if (member == null)
            return Result.Failure<MemberSummaryDto>(UserErrors.UserNotFound);

        return Result.Success(member);
    }
}