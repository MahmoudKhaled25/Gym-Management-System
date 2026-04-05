namespace Gym_Management_System.Entities;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
        Id = Guid.CreateVersion7().ToString();
    }
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}
