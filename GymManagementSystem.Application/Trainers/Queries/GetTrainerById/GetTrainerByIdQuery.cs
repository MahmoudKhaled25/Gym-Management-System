using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Queries.GetTrainerById;

public record GetTrainerByIdQuery(string TrainerId) : IRequest<Result<TrainerDto>>;
