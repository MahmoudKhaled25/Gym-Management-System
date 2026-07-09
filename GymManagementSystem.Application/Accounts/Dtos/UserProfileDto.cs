using GymManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Dtos;

public record UserProfileDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    float Weight,
    float Height,
    string? ProfileImageUrl,
    Gender Gender,
    IEnumerable<string> Roles
);