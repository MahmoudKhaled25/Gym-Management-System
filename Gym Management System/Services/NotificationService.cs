using GymManagementSystem.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace GymManagementSystem.Services;

public class NotificationService(IOptions<TwilioSettings> options) : INotificationService 
{
    private readonly TwilioSettings _twilioSettings = options.Value;

    public async Task SendWhatsAppAsync(string to, string message)
    {
        var accountSid = _twilioSettings.AccountSid;
        var authToken = _twilioSettings.AuthToken;
        var from = _twilioSettings.WhatsappFromNumber;
        to = FormatEgyptianPhoneNumber(to);
        TwilioClient.Init(accountSid, authToken);

        await MessageResource.CreateAsync(
            from: new PhoneNumber(from),
            to: new PhoneNumber($"whatsapp:{to}"),
            body: message
            );
    }
    private string FormatEgyptianPhoneNumber(string phoneNumber)
    {
        phoneNumber = phoneNumber.Trim();

        if (phoneNumber.StartsWith("0"))
        {
            phoneNumber = "+2" + phoneNumber;
        }

        return phoneNumber;
    }
}
