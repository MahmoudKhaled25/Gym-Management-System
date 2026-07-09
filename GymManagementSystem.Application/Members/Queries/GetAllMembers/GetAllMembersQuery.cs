using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Queries.GetAllMembers;

public record GetAllMembersQuery(RequestFilters Filters) : IRequest<Result<PaginatedList<MemberSummaryDto>>>;
