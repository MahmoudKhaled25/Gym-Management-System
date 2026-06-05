using GymManagementSystem.Enums;

namespace GymManagementSystem.Contracts.Admin;

public record AdminResponse(string Id, string Name, string Email,string? PhoneNumber,Gender Gender, bool IsActive);

