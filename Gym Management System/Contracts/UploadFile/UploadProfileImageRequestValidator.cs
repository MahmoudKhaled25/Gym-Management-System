using GymManagementSystem.Contracts.UploadFile.Common;

namespace GymManagementSystem.Contracts.UploadFile;

public class UploadProfileImageRequestValidator : AbstractValidator<UploadProfileImageRequest>
{
    public UploadProfileImageRequestValidator()
    {
        RuleFor(x => x.Image)
            .SetValidator(new FileExtensionValidator())
            .SetValidator(new FileSignatureValidator())
            .SetValidator(new FileSizeValidator());
    }
}
