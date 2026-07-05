using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand(
    string UserId,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    float Weight,
    float Height,
    string? PhoneNumber) : IRequest<Result>;

