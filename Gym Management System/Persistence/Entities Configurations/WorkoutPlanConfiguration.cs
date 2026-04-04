using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym_Management_System.Persistence.Entities_Configurations;

public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
{
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasOne(x => x.Trainer)
            .WithMany(x => x.WorkoutPlans)
            .HasForeignKey(x => x.TrainerId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.WorkoutPlans)
            .HasForeignKey(x => x.UserId);
    }
}
