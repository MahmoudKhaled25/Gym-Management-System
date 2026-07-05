using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Commands.DeleteProfileImage;

public record DeleteProfileImageCommand(string UserId) : IRequest<Result>;
