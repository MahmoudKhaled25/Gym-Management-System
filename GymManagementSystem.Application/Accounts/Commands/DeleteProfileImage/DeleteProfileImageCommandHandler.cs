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

namespace GymManagementSystem.Application.Accounts.Commands.DeleteProfileImage;

public class DeleteProfileImageCommandHandler(IUnitOfWork unitOfWork,IFileStorageService fileStorageService) : IRequestHandler<DeleteProfileImageCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<Result> Handle(DeleteProfileImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<ApplicationUser>()
            .SingleOrDefaultAsync(x => x.Id == request.UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        if (user.ProfileImageId is null)
            return Result.Failure(UserErrors.NoProfileImage);

        var image = await _unitOfWork.Repository<UploadedFile>()
            .FirstOrDefaultAsync(x => x.Id == user.ProfileImageId, cancellationToken);

        if (image is not null)
            await _fileStorageService.DeleteAsync(image, cancellationToken);

        user.ProfileImageId = null;
        _unitOfWork.Repository<ApplicationUser>().Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
