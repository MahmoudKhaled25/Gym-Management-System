using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;

namespace GymManagementSystem.Application.Trainers.Queries.GetTrainerById;

public class GetTrainerByIdQueryHandler(IUnitOfWork unitOfWork,IIdentityService applicationUserRepository) : IRequestHandler<GetTrainerByIdQuery, Result<TrainerDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdentityService _applicationUserRepository = applicationUserRepository;

    public async Task<Result<TrainerDto>> Handle(GetTrainerByIdQuery request, CancellationToken cancellationToken)
    {
        var trainer = await _unitOfWork.Repository<Trainer>()
             .FirstOrDefaultAsync(x => x.UserId == request.TrainerId);

        if (trainer is null)
        {
            return Result.Failure<TrainerDto>(TrainerErrors.TrainerNotFound);
        }
        var roles = await _applicationUserRepository.GetRolesAsync(trainer.UserId);
        var result = new TrainerDto(trainer.UserId,trainer.ApplicationUser!.FirstName,trainer.ApplicationUser.LastName,trainer.Specialization,trainer.IsActive,roles.AsEnumerable());

        return Result.Success(result);
    }
}
