using Gym_Management_System.Persistence;
using GymManagementSystem.Entities;

namespace GymManagementSystem.Services;

public class FileStorageService(IWebHostEnvironment environment, ApplicationDbContext context) : IFileStorageService
{
    private readonly ApplicationDbContext _context = context;
    private readonly string _profileImagesPath =Path.Combine(environment.WebRootPath,"uploads","profile-images");
    public async Task<UploadedFile> SaveProfileImageAsync(IFormFile file,CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_profileImagesPath);
        var extension =Path.GetExtension(file.FileName);
        var storedName =$"{Guid.CreateVersion7()}{extension}";
        var physicalPath =Path.Combine(_profileImagesPath, storedName);
        using var stream = File.Create(physicalPath);
        await file.CopyToAsync(stream, cancellationToken);
        var uploadedFile = new UploadedFile
        {
            OriginalFileName = file.FileName,
            StoredFileName = storedName,
            ContentType = file.ContentType,
            Extension = extension,
            Size = file.Length,
            RelativePath =$"/uploads/profile-images/{storedName}"
        };
        _context.UploadedFiles.Add(uploadedFile);
        await _context.SaveChangesAsync(cancellationToken);
        return uploadedFile;
    }
    public async Task DeleteAsync(UploadedFile file, CancellationToken cancellationToken = default)
    {
        var physicalPath = Path.Combine(environment.WebRootPath,file.RelativePath.TrimStart('/'));
        if (File.Exists(physicalPath))
          File.Delete(physicalPath);
        
        _context.UploadedFiles.Remove(file);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
