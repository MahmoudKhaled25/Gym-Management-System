namespace Gym_Management_System.Contracts.Account;

public record UserProfileResponse(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    float Weight,
    float Height,
    TrainerResponse? Trainer,
    IEnumerable<string> Roles
);

public record TrainerResponse(
    string FirstName,
    string LastName,
    string Specialization,
    bool IsActive
);