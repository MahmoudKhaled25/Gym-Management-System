using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Commands.Toggle_Status;

public record PlanToggleStatusCommand(int Id) : IRequest<Result>;
