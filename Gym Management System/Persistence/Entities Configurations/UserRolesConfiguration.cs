using Gym_Management_System.Abstractions.Consts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym_Management_System.Persistence.Entities_Configurations;

public class UserRolesConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(new IdentityUserRole<string>
        {
            UserId = DefaultUsers.AdminId,
            RoleId = DefaultRoles.Admin.Id
        });
    }
}
