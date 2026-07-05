using GymManagementSystem.Application.Accounts.Dtos;
using GymManagementSystem.Application.UploadFiles;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Queries.GetProfile;

public class GetProfileQueryHandler(IAccountQueries accountQueries) : IRequestHandler<GetProfileQuery, Result<UserProfileDto>>
{
    private readonly IAccountQueries _accountQueries = accountQueries;

    public async Task<Result<UserProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await _accountQueries.GetProfileAsync(request.UserId, cancellationToken);
        return result;
    }
}
