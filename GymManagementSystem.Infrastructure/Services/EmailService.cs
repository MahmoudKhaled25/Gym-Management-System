using GymManagementSystem.Application.Auth.Abstractions;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Infrastructure.Settings;
using GymManagementSystem.Infrastructure.Templates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;

namespace GymManagementSystem.Infrastructure.Services;

public class EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly ILogger<EmailService> _logger = logger;

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
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

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
