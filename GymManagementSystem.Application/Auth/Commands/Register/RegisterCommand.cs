using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Commands.Register;

public record RegisterCommand
(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    Gender Gender,
    string PhoneNumber

) : IRequest<Result>;
