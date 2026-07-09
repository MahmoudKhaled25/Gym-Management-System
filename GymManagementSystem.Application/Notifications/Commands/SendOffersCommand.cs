using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Notifications.Commands;

public record SendOffersCommand(string Message) : IRequest;
