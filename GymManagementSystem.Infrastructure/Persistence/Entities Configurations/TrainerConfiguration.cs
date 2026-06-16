using GymManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.Infrastructure.Persistence.Entities_Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.Specialization)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(x => x.ApplicationUser)
            .WithOne(x => x.Trainer)
            .HasForeignKey<Trainer>(x => x.UserId);
    }
}
