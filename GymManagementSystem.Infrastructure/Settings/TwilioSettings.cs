using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Infrastructure.Settings;

public class TwilioSettings
{
    [Required]
    public string AccountSid { get; set; } = string.Empty;

    [Required]
    public string AuthToken { get; set; } = string.Empty;

    [Required]
    public string WhatsappFromNumber { get; set; } = string.Empty;
}
