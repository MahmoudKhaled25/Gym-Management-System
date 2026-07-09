using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Interfaces;

public interface INotificationService
{
    Task SendWhatsAppAsync(string to, string message);
}
