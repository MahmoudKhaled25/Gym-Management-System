using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Queries.GetTrainerMembers;

public record GetTrainerMembersQuery(string TrainerId) : IRequest<Result<IEnumerable<TrainerMembersDto>>>;

