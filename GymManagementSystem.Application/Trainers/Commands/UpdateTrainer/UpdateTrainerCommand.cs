using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Commands.UpdateTrainer;

public record UpdateTrainerCommand(string TrainerId,
  string FirstName,
 string LastName,
 string Specialization) : IRequest<Result>;

