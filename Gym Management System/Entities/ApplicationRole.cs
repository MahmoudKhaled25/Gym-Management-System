namespace Gym_Management_System.Entities;

public class ApplicationRole : IdentityRole
{
    
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}
