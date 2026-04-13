namespace Gym_Management_System.Contracts.Auth;

public record AuthResponse
(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpiresIn,
    IEnumerable<string> Roles,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);