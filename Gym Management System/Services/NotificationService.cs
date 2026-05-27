using GymManagementSystem.Settings;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace GymManagementSystem.Services;

public class NotificationService : INotificationService
{
    private readonly TwilioSettings _twilioSettings;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IOptions<TwilioSettings> options,ILogger<NotificationService> logger)
    {
        _twilioSettings = options.Value;
        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
        _logger = logger;
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