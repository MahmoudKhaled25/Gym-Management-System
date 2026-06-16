using GymManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Domain.Entities;

public class ApplicationUser : IdentityUser
{
   
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public float Weight { get; set; }
    public float Height { get; set; }
    public DateTime CreatedAt { get; set; }

    [Required]
    public Gender Gender { get; set; }

    public Guid? ProfileImageId { get; set; }
    public UploadedFile? ProfileImage { get; set; }
    // Navigation Properties

    public Trainer? Trainer { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<ProgressLog> ProgressLogs { get; set; } = [];
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = [];
    public List<RefreshToken> RefreshTokens { get; set; } = [];

}