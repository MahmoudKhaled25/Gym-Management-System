namespace Gym_Management_System.Contracts.Account;

public record ChangeUserPasswordRequest(string OldPassword, string NewPassword);