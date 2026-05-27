using Gym_Management_System.Persistence;
using GymManagementSystem.Entities;

namespace GymManagementSystem.Services;

public class FileStorageService(IWebHostEnvironment environment, ApplicationDbContext context,ILogger<FileStorageService> logger) : IFileStorageService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<FileStorageService> _logger = logger;
    private readonly string _profileImagesPath =Path.Combine(environment.WebRootPath,"uploads","profile-images");
    public async Task<UploadedFile> SaveProfileImageAsync(IFormFile file,CancellationToken cancellationToken = default)
    {
        try
        {
            Directory.CreateDirectory(_profileImagesPath);
            var extension = Path.GetExtension(file.FileName);
            var storedName = $"{Guid.CreateVersion7()}{extension}";
            var physicalPath = Path.Combine(_profileImagesPath, storedName);
            using var stream = File.Create(physicalPath);
            await file.CopyToAsync(stream, cancellationToken);
            var uploadedFile = new UploadedFile
            {
                OriginalFileName = file.FileName,
                StoredFileName = storedName,
                ContentType = file.ContentType,
                Extension = extension,
                Size = file.Length,
                RelativePath = $"/uploads/profile-images/{storedName}"
            };
            _context.UploadedFiles.Add(uploadedFile);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Profile image saved: {FileName}", uploadedFile.OriginalFileName);
            return uploadedFile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save image: {FileName}", file.FileName);
            throw;
        }

    }
    public async Task DeleteAsync(UploadedFile file, CancellationToken cancellationToken = default)
    {
        var physicalPath = Path.Combine(environment.WebRootPath,file.RelativePath.TrimStart('/'));
        if (File.Exists(physicalPath))
          File.Delete(physicalPath);
        
        _context.UploadedFiles.Remove(file);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Deleted file: {FileName}", file.OriginalFileName);
    }
}
