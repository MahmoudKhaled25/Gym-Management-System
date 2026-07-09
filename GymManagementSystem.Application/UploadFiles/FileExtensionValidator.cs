using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.UploadFiles;

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
