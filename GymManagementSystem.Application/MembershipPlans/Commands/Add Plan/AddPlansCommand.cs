using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Commands.Add_Plan;

public record AddPlansCommand(string Name, string Description, decimal Price, int DurationInDays) : IRequest<Result<MembershipPlanDto>>;
