using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Trainer;

namespace Gym_Management_System.Services;

public interface ITrainerService
{
    Task<Result<IEnumerable<GetTrainerResponse>>> GetAllTrainersAsync();

    Task<Result<IEnumerable<GetTrainerResponse>>> GetActiveTrainersAsync();

    Task<Result<GetTrainerResponse>> GetTrainerByIdAsync(string trainerId, CancellationToken cancellationToken = default);

    Task<Result<GetTrainerResponse>> AddTrainerAsync(AddTrainerRequest request, CancellationToken cancellationToken = default);
}
