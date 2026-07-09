using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Dtos;

public record UpdateUserProfileDto(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    float Weight,
    float Height,
    string? PhoneNumber
);
