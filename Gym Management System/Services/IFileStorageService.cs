using GymManagementSystem.Entities;

namespace GymManagementSystem.Services;

public interface IFileStorageService
{
    Task<UploadedFile> SaveProfileImageAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task DeleteAsync(UploadedFile file, CancellationToken cancellationToken = default);
}
