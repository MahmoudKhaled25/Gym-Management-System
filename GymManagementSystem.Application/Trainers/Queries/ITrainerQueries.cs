using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Application.Trainers.Queries.GetAllTrainers;

namespace GymManagementSystem.Application.Trainers.Queries;

public interface ITrainerQueries
{
    Task<PaginatedList<TrainerDto>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken);
    Task<IEnumerable<TrainerDto>> GetActiveAsync(CancellationToken cancellationToken);
}
