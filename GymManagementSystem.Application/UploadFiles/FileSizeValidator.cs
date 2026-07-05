using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.UploadFiles;

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
