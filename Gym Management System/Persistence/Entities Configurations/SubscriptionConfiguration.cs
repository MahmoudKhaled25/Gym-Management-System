using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym_Management_System.Persistence.Entities_Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
            builder.Property(x => x.Status)
                .HasConversion<string>();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.MembershipPlan)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.MembershipPlanId);

        builder.HasOne(x => x.Trainer)
            .WithMany()
            .HasForeignKey(x => x.TrainerId);
    }
}
