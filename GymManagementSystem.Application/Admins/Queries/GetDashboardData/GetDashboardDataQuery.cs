using GymManagementSystem.Application.Admins.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Queries.GetDashboardData;

public record GetDashboardDataQuery() : IRequest<Result<DashboardDataDto>>;
