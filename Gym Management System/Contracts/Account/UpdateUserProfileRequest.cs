namespace Gym_Management_System.Contracts.Account;

public record UpdateUserProfileRequest(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    float Weight,
    float Height,
    string? PhoneNumber
);
