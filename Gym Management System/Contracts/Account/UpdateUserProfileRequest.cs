namespace Gym_Management_System.Contracts.Account;

public record UpdateUserProfileRequest(
    string Email,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    float Weight,
    float Height
);
