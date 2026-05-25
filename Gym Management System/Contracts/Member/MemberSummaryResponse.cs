using GymManagementSystem.Enums;

namespace GymManagementSystem.Contracts.Member;

public record MemberSummaryResponse(
    string Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    Gender Gender,
    string? TrainerName,
    string? ActivePlanName,
    bool IsActive
);
