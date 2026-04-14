using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Member;

namespace Gym_Management_System.Services;

public interface IMemberService
{
    Task<Result<IEnumerable<UserProfileResponse>>> GetAllMembersAsync();

    Task<Result<IEnumerable<UserProfileResponse>>> GetActiveMembersAsync(string memberId, CancellationToken cancellationToken = default!);


    Task<Result<UserProfileResponse>> GetMembersAsync(string memberId,CancellationToken cancellationToken = default!);

    Task<Result<UserProfileResponse>> AddMemberAsync(AddMemberRequest request, CancellationToken cancellationToken = default!);

    Task<Result> ToggleStatusAsync(string memberId, CancellationToken cancellationToken = default!);


}
