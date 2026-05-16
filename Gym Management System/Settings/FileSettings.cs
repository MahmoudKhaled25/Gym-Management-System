namespace GymManagementSystem.Settings;

public static class FileSettings
{
    public const int MaxFileSizeInMB = 1;

    public const int MaxFileSizeInBytes =
        MaxFileSizeInMB * 1024 * 1024;

    public static readonly string[] AllowedExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png"
    ];
    public static readonly Dictionary<string, List<byte[]>>
        AllowedSignatures = new()
        {
            [".jpg"] =
        [
            [0xFF, 0xD8, 0xFF]
        ],

            [".jpeg"] =
        [
            [0xFF, 0xD8, 0xFF]
        ],

            [".png"] =
        [
            [0x89, 0x50, 0x4E, 0x47]
        ]
        };
        }
