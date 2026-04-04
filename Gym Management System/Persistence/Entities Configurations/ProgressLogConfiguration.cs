using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym_Management_System.Persistence.Entities_Configurations;

public class ProgressLogConfiguration : IEntityTypeConfiguration<ProgressLog>
{
    public void Configure(EntityTypeBuilder<ProgressLog> builder)
    {
        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.Property(x => x.Weight)
            .HasColumnType("decimal(5,2)");

        builder.HasOne(x => x.User)
            .WithMany(x => x.ProgressLogs)
            .HasForeignKey(x => x.UserId);
    }
}
