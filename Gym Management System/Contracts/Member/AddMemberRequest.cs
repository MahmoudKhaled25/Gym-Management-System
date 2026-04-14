namespace Gym_Management_System.Contracts.Member;

public record AddMemberRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    float Weight,
    float Height,
    string? PhoneNumber
);
