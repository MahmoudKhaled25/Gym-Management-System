using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Queries.GetMyMembers;

public record GetMyMembersQuery(string TrainerId) : IRequest<Result<IEnumerable<TrainerMembersDto>>>;
