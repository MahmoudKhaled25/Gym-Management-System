using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;

namespace GymManagementSystem.Application.MembershipPlans.Queries.Get_All_Plans;

public record GetAllPlansQuery() : IRequest<Result<IEnumerable<MembershipPlanDto>>>;
