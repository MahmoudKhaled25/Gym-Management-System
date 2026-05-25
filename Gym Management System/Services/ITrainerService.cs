using Gym_Management_System.Abstractions;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Trainer;
using GymManagementSystem.Abstractions;
using GymManagementSystem.Contracts.Common;

namespace Gym_Management_System.Services;

public interface ITrainerService
{
    Task<Result<PaginatedList<GetTrainerResponse>>> GetAllTrainersAsync(RequestFilters requestFilters,CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<GetTrainerResponse>>> GetActiveTrainersAsync();

    Task<Result<GetTrainerResponse>> GetTrainerByIdAsync(string trainerId, CancellationToken cancellationToken = default);

    Task<Result<GetTrainerResponse>> AddTrainerAsync(AddTrainerRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateTrainerAsync(string trainerId,UpdateTrainerRequest request, CancellationToken cancellationToken = default);

    Task<Result> ToggleStatusAsync(string trainerId,CancellationToken cancellationToken = default);
}
