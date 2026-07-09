using GymManagementSystem.Application.Accounts.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Queries;

public interface IAccountQueries
{
    Task<Result<UserProfileDto>> GetProfileAsync(string userId, CancellationToken cancellationToken);
}
