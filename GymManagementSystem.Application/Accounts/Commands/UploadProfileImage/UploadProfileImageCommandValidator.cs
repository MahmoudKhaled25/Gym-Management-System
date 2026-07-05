using FluentValidation;
using GymManagementSystem.Application.UploadFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Commands.UploadProfileImage;

public class UploadProfileImageCommandValidator : AbstractValidator<UploadProfileImageCommand>
{
    public UploadProfileImageCommandValidator()
    {
        RuleFor(x => x.Image)
           .SetValidator(new FileExtensionValidator())
           .SetValidator(new FileSignatureValidator())
           .SetValidator(new FileSizeValidator());
    }
}
