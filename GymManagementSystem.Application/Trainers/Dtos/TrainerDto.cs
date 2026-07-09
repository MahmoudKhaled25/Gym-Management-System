using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Dtos;

public record TrainerDto(string Id,
        string FirstName,
        string LastName,
        string Specialization,
        bool IsActive,
        IEnumerable<string> Roles);
public record TrainerMembersDto(
    string MemberId,
    string FullName,
    string? PhoneNumber,
    string MembershipPlanName,
    DateOnly SubscriptionEndDate
);