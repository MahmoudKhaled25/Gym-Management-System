using GymManagementSystem.Application.Admins.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Queries.GetAllAdmins;

public record GetAllAdminsQuery() : IRequest<Result<IEnumerable<AdminDto>>>;
