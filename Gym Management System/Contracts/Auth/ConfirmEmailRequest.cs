namespace GymManagementSystem.Contracts.Auth;

public record ConfirmEmailRequest(
    string Email,
    string Token
);

