using GymManagementSystem.Application.Accounts.Dtos;
using GymManagementSystem.Application.Accounts.Queries;
using GymManagementSystem.Application.UploadFiles;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Enums;
using GymManagementSystem.Domain.Errors;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Infrastructure.Queries;

public class AccountQueries(UserManager<ApplicationUser> userManager) : IAccountQueries
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<UserProfileDto>> GetProfileAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
                   .Include(u => u.Trainer)
                   .Include(u => u.ProfileImage)
                   .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Result.Failure<UserProfileDto>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(user);

        var profileImageUrl = user.ProfileImageId != null
            ? user.ProfileImage?.RelativePath
            : user.Gender == Gender.Male
                ? FileSettings.MaleDefaultImage
                : FileSettings.FemaleDefaultImage;

        var response = user.Adapt<UserProfileDto>() with
        {
            Roles = roles,
            ProfileImageUrl = profileImageUrl
        };

        return Result.Success(response);
    }
}
