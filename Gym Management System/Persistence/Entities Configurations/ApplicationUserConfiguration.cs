using Gym_Management_System.Abstractions.Consts;
using Gym_Management_System.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym_Management_System.Persistence.Entities_Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Weight)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.Height)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasData(new ApplicationUser
        {
            Id = DefaultUsers.AdminId,
            Email = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            UserName = DefaultUsers.AdminEmail,
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            PasswordHash = DefaultUsers.AdminPasswordHash,
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            FirstName = "Admin",
            LastName = "Admin",
            EmailConfirmed = true,
        });

        builder.OwnsMany(x => x.RefreshTokens, rt =>
        {
            rt.ToTable("RefreshTokens");
            rt.WithOwner().HasForeignKey("UserId");
            rt.HasKey("Id");

        });
    }
}
