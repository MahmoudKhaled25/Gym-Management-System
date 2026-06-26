using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Commands.AddTrainer;

public record AddTrainerCommand(string Email,
    string Password,
    string FirstName,
    string LastName,
    string Specialization) : IRequest<Result<TrainerDto>>;
