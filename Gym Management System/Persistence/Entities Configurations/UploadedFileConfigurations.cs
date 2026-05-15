using GymManagementSystem.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.Persistence.Entities_Configurations;

public class UploadedFileConfigurations : IEntityTypeConfiguration<UploadedFile>
{
    public void Configure(EntityTypeBuilder<UploadedFile> builder)
    {
        builder.Property(x => x.OriginalFileName)
            .HasMaxLength(255);

        builder.Property(x => x.StoredFileName)
            .HasMaxLength(255);

        builder.Property(x => x.ContentType)
            .HasMaxLength(100);

        builder.Property(x => x.Extension)
            .HasMaxLength(10);

        builder.Property(x => x.RelativePath)
            .HasMaxLength(500);

        builder.HasIndex(x => x.StoredFileName)
            .IsUnique();
    }
}
