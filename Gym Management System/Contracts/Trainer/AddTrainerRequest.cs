namespace Gym_Management_System.Contracts.Trainer;

public record AddTrainerRequest
(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Specialization
);
