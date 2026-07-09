using GymManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.Infrastructure.Persistence.Entities_Configurations;

public class WorkoutPlanExerciseConfiguration : IEntityTypeConfiguration<WorkoutPlanExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutPlanExercise> builder)
    {
        builder.HasOne(x => x.WorkoutPlan)
            .WithMany(x => x.WorkoutPlanExercises)
            .HasForeignKey(x => x.WorkoutPlanId);

        builder.HasOne(x => x.Exercise)
            .WithMany(x => x.WorkoutPlanExercises)
            .HasForeignKey(x => x.ExerciseId);
    }
}
