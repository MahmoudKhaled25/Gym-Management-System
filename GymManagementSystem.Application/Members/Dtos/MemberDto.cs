using GymManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Dtos;

public record MemberSummaryDto(
    string Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    Gender Gender,
    string? TrainerName,
    string? ActivePlanName,
    bool IsActive
);
public record AddMemberDto
(
    string Id,
    string Email,
    string FirstName,
    string LastName
);