using Gym_Management_System.Contracts.Exercise;
using GymManagementSystem.Abstractions;
using GymManagementSystem.Contracts.Common;

namespace Gym_Management_System.Services;

public interface IExerciseService
{
    Task<Result<PaginatedList<ExerciseResponse>>> GetAllAsync(RequestFilters filters,CancellationToken cancellationToken);
    Task<Result<ExerciseResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Result<ExerciseResponse>> AddAsync(ExerciseRequest request, CancellationToken cancellationToken);

    Task<Result> UpdateAsync(int id,ExerciseRequest request, CancellationToken cancellationToken);

    Task<Result> ToggleStatusAsync(int id,CancellationToken cancellationToken);

}
