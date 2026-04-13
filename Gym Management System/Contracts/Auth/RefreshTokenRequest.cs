namespace Gym_Management_System.Contracts.Auth;

public record RefreshTokenRequest
(
    string Token,
    string RefreshToken
);
