using GymManagementSystem.Application.Interfaces;
using GymManagementSystem.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace GymManagementSystem.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly TwilioSettings _twilioSettings;

    public NotificationService(
        ILogger<NotificationService> logger,
        IOptions<TwilioSettings> twilioOptions)
    {
        _logger = logger;
        _twilioSettings = twilioOptions.Value;

        TwilioClient.Init(
            _twilioSettings.AccountSid,
            _twilioSettings.AuthToken);
    }

    public async Task SendWhatsAppAsync(string to, string message)
    {
        try
        {
            to = FormatEgyptianPhoneNumber(to);
            await MessageResource.CreateAsync(
                from: new PhoneNumber(_twilioSettings.WhatsappFromNumber),
                to: new PhoneNumber($"whatsapp:{to}"),
                body: message
            );
            _logger.LogInformation("WhatsApp sent to {Phone}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send WhatsApp message to {Phone}", to);
        }
    }

    private string FormatEgyptianPhoneNumber(string phoneNumber)
    {
        phoneNumber = phoneNumber.Trim();

        if (phoneNumber.StartsWith("00"))
            return "+" + phoneNumber.Substring(2);

        if (phoneNumber.StartsWith("0"))
            return "+2" + phoneNumber;

        if (!phoneNumber.StartsWith("+"))
            return "+2" + phoneNumber;

        return phoneNumber;
    }
}
