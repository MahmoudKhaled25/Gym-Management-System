using GymManagementSystem.Enums;

namespace GymManagementSystem.Contracts.Admin;

public record AddAdminRequest(string FirstName, string LastName, string Email,string Password, string? PhoneNumber, Gender Gender);

