using GymManagementSystem.Application.Auth.Abstractions;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Infrastructure.Settings;
using GymManagementSystem.Infrastructure.Templates;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GymManagementSystem.Infrastructure.Services;

public class EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger, IWebHostEnvironment environment ) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly ILogger<EmailService> _logger = logger;
    private readonly IWebHostEnvironment _environment = environment;

    public async Task SendConfirmationEmailAsync(ApplicationUser user, string confirmationLink)
    {
        var body = EmailTemplates.GetConfirmationEmailBody(user.FirstName, confirmationLink);

        await SendEmailAsync(
            user.Email!,
            "Confirm your email - Gym Management",
            body);

    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            _emailSettings.DisplayName,
            _emailSettings.UserName));

        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        _logger.LogInformation("Sending Email To {email}", to);
        if (_environment.IsDevelopment())
        {
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.CheckCertificateRevocation = false;
        }

        await client.ConnectAsync(
            _emailSettings.Host,
            _emailSettings.Port,
            SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(
            _emailSettings.UserName,
            _emailSettings.Password);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendResetPasswordEmailAsync(ApplicationUser user, string otpCode)
    {
        var body = EmailTemplates.GetOtpEmailBody(user.FirstName, otpCode);
        await SendEmailAsync(
            user.Email!,
            "Reset your password - Gym Management",
            body);
    }
}
