using GymManagementSystem.Settings;

namespace GymManagementSystem.Contracts.UploadFile.Common;

public class FileExtensionValidator : AbstractValidator<IFormFile>
{
    public FileExtensionValidator()
    {
        RuleFor(file => file)
            .Must(file =>
            {
                if (file is null)
                    return true;

                var extension =
                    Path.GetExtension(file.FileName);

                return FileSettings.AllowedExtensions
                    .Contains(
                        extension,
                        StringComparer.OrdinalIgnoreCase);
            })
            .WithMessage("Invalid file extension");
    }
}