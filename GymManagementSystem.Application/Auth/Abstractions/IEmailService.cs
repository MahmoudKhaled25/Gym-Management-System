using GymManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendConfirmationEmailAsync(ApplicationUser user, string confirmationLink);
    Task SendResetPasswordEmailAsync(ApplicationUser user, string otpCode);
}
