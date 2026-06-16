using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;

namespace GymManagementSystem.Application.MembershipPlans.Commands.Update_Plan;

public record UpdatePlanCommand(int Id,string Name, string Description, decimal Price, int DurationInDays) : IRequest<Result<MembershipPlanDto>>;
