using GymManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.Application.Interfaces;

public interface IFileStorageService
{
    Task<UploadedFile> SaveProfileImageAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task DeleteAsync(UploadedFile file, CancellationToken cancellationToken = default);
}
