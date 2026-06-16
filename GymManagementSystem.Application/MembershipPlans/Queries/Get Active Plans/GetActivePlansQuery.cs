using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Queries.Get_Active_Plans;

public record GetActivePlansQuery() : IRequest<Result<IEnumerable<MembershipPlanDto>>>;

