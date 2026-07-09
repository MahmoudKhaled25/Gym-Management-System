using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Queries.GetMemberById;

public record GetMemberByIdQuery(string MemberId) : IRequest<Result<MemberSummaryDto>>;
