using System.ComponentModel.DataAnnotations;

namespace Gym_Management_System.Authentication;

public class JwtOptions
{
    public static readonly string SectionName = "Jwt";

    [Required]
    public string Key { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ExpiryMinutes { get; set; }
}
