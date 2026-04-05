using Gym_Management_System.Abstractions.Consts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym_Management_System.Persistence.Entities_Configurations;

public class RolesConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
       builder.HasData(new ApplicationRole
        {
            Id = DefaultRoles.Admin.Id,
            Name = DefaultRoles.Admin.Name,
            NormalizedName = DefaultRoles.Admin.Name.ToUpper(),
           ConcurrencyStamp = DefaultRoles.Admin.ConcurrencyStamp,
           IsDefault = false,
            IsDeleted = false,
        },
        new ApplicationRole
        {
            Id = DefaultRoles.Member.Id,
            Name = DefaultRoles.Member.Name,
            NormalizedName = DefaultRoles.Member.Name.ToUpper(),
            ConcurrencyStamp = DefaultRoles.Member.ConcurrencyStamp,
            IsDefault = true,
            IsDeleted = false,
        },new ApplicationRole
        {
            Id =DefaultRoles.Trainer.Id,
            Name = DefaultRoles.Trainer.Name,
            NormalizedName = DefaultRoles.Trainer.Name.ToUpper(),
            ConcurrencyStamp = DefaultRoles.Trainer.ConcurrencyStamp,
            IsDefault = false,
            IsDeleted = false,
        }
        );
    }
}
