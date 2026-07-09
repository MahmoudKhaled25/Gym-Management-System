using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Infrastructure.Settings;

public class EmailSettings
{
    public static string SectionName = "EmailSettings";
    [Required] 
    public string Host { get; set; } = string.Empty;
    [Range(100, 999)]
    public int Port { get; set; }
    [Required, EmailAddress]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string DisplayName { get; set; } = string.Empty;
}
