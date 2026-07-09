using GymManagementSystem.Application.Interfaces;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace GymManagementSystem.Application.Accounts.Commands.UploadProfileImage;

public class UploadProfileImageCommandHandler(IFileStorageService fileStorageService,IUnitOfWork unitOfWork) : IRequestHandler<UploadProfileImageCommand, Result<string>>
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<string>> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<ApplicationUser>()
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<string>(UserErrors.UserNotFound);

        if (user.ProfileImageId is not null)
        {
            var oldImage = await _unitOfWork.Repository<UploadedFile>()
                .FirstOrDefaultAsync(x => x.Id == user.ProfileImageId, cancellationToken);

            if (oldImage is not null)
                await _fileStorageService.DeleteAsync(oldImage, cancellationToken);
        }

        var uploadedFile = await _fileStorageService.SaveProfileImageAsync(request.Image, cancellationToken);

        user.ProfileImageId = uploadedFile.Id;
        _unitOfWork.Repository<ApplicationUser>().Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(uploadedFile.RelativePath);
    }
}
