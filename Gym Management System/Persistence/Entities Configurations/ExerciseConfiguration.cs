
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym_Management_System.Persistence.Entities_Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.MuscleGroup)
            .HasMaxLength(100);
    }
}
