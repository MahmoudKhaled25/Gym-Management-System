using GymManagementSystem.Settings;

namespace GymManagementSystem.Contracts.UploadFile.Common;

public class FileSizeValidator : AbstractValidator<IFormFile>
{
    public FileSizeValidator()
    {
        RuleFor(file => file)
            .Must(file =>
                file is null ||
                file.Length <= FileSettings.MaxFileSizeInBytes)
            .WithMessage(
                $"File size must be less than {FileSettings.MaxFileSizeInMB} MB");
    }
}
