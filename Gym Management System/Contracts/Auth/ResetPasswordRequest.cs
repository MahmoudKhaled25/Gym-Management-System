namespace Gym_Management_System.Contracts.Auth;

public record ResetPasswordRequest
(   string Email,
    string Code,
    string NewPassword
);
