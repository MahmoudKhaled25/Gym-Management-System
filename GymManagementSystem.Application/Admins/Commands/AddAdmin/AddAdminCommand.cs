using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Commands.AddAdmin;

public record AddAdminCommand(string FirstName, string LastName, string Email, string Password, string? PhoneNumber, Gender Gender) : IRequest<Result>;

