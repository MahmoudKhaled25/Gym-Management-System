using GymManagementSystem.Settings;

namespace GymManagementSystem.Contracts.UploadFile.Common;

public class FileSignatureValidator : AbstractValidator<IFormFile>
{
    public FileSignatureValidator()
    {
        RuleFor(file => file)
        .Must(HaveValidSignature)
        .WithMessage("Invalid file content");
    }
    private static bool HaveValidSignature(IFormFile? file)
    {
        if (file is null)
            return true;

        var extension =
            Path.GetExtension(file.FileName);

        if (!FileSettings.AllowedSignatures
                .TryGetValue(extension, out var signatures))
            return false;

        using var stream = file.OpenReadStream();
        using var reader = new BinaryReader(stream);

        var headerBytes = reader.ReadBytes(
            signatures.Max(x => x.Length));

        return signatures.Any(signature =>
            headerBytes
                .Take(signature.Length)
                .SequenceEqual(signature));
    }
}