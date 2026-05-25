using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Member;
using GymManagementSystem.Abstractions;
using GymManagementSystem.Contracts.Common;
using GymManagementSystem.Contracts.Member;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Management_System.Services;

public interface IMemberService
{
    Task<Result<PaginatedList<MemberSummaryResponse>>> GetAllMembersAsync(RequestFilters filters, CancellationToken cancellationToken = default);

    Task<Result<PaginatedList<MemberSummaryResponse>>> GetActiveMembersAsync(RequestFilters filters, CancellationToken cancellationToken);


    Task<Result<UserProfileResponse>> GetMemberAsync(string memberId, CancellationToken cancellationToken = default!);

    Task<Result<UserProfileResponse>> AddMemberAsync(AddMemberRequest request, CancellationToken cancellationToken = default!);

    Task<Result> ToggleStatusAsync(string memberId, CancellationToken cancellationToken = default!);


}
