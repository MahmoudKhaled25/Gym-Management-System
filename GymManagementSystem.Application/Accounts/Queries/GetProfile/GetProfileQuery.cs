using GymManagementSystem.Application.Accounts.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Queries.GetProfile;

public record GetProfileQuery(string UserId) : IRequest<Result<UserProfileDto>>;
