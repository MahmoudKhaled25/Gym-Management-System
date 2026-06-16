using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Queries.Get_By_Id;

public record GetPlanByIdQuery(int Id) : IRequest<Result<MembershipPlanDto>>;
