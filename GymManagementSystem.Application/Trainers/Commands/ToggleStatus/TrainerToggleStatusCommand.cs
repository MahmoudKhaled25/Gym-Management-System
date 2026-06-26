using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Commands.ToggleStatus;

public record TrainerToggleStatusCommand(string TrainerId) : IRequest<Result>;
