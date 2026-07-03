using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;

namespace GymManagementSystem.Application.Members.Queries.GetActiveMembers;

public record GetActiveMembersQuery(RequestFilters Filters) : IRequest<Result<PaginatedList<MemberSummaryDto>>>;
