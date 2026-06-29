using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Commands.MemberToggleStatus;

public record MemberToggleStatusCommand(string MemberId) : IRequest<Result>;

