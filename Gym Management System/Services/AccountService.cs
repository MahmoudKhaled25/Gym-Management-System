using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using GymManagementSystem.Services;

namespace Gym_Management_System.Services;

public class AccountService(ApplicationDbContext context,UserManager<ApplicationUser> userManager, IFileStorageService fileStorageService) : IAccountService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
                    .Include(u => u.Trainer)
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(user);
        var response = user.Adapt<UserProfileResponse>() with { Roles = roles };

        return Result.Success(response);

    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default)
    {

        var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Result.Failure(UserErrors.UserNotFound);

        request.Adapt(user);

        await _userManager.UpdateAsync(user);

        return Result.Success();

    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangeUserPasswordRequest request, CancellationToken cancellationToken = default!)
    {
        var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Result.Failure(UserErrors.UserNotFound);

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword,request.NewPassword);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
          
        }

        return Result.Success();

    }
 
        public async Task<Result<string>> UploadProfileImageAsync(string userId,IFormFile image,CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure<string>(UserErrors.UserNotFound);

            if (user.ProfileImageId is not null)
            {
                var oldImage = await _context.UploadedFiles
                    .FirstOrDefaultAsync(x => x.Id == user.ProfileImageId, cancellationToken);

                if (oldImage is not null)
                    await _fileStorageService.DeleteAsync(oldImage, cancellationToken);
            }

            var uploadedFile = await _fileStorageService.SaveProfileImageAsync(image, cancellationToken);

            user.ProfileImageId = uploadedFile.Id;
            await _userManager.UpdateAsync(user);

            return Result.Success(uploadedFile.RelativePath);
        }
    
    public async Task<Result> DeleteProfileImageAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        if (user.ProfileImageId is null)
            return Result.Failure(UserErrors.NoProfileImage);

        var image = await _context.UploadedFiles
            .FirstOrDefaultAsync(x => x.Id == user.ProfileImageId, cancellationToken);

        if (image is not null)
            await _fileStorageService.DeleteAsync(image, cancellationToken);

        user.ProfileImageId = null;
        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
}
