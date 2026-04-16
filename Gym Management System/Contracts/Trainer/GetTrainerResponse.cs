namespace Gym_Management_System.Contracts.Trainer;

public record GetTrainerResponse
(
    string Id,
    string FirstName,
    string LastName,
    string Specialization,
    bool IsActive,
    IEnumerable<string> Roles

    //to do : be filled with member names associated with the trainer
    // IEnumerable<string> MembersNames

);
