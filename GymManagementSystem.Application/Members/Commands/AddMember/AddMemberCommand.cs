using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Commands.AddMember;

public record AddMemberCommand(string Email,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    float Weight,
    float Height,
    Gender Gender,
    string? PhoneNumber) : IRequest<Result<AddMemberDto>>;
