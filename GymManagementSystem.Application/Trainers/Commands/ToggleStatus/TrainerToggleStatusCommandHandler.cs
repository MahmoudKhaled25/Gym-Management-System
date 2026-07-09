using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Commands.ToggleStatus;

public class TrainerToggleStatusCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : IRequestHandler<TrainerToggleStatusCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _allTrainersCacheKey = "Trainers_All";
    private const string _activeTrainersCacheKey = "Trainers_Active";
    public async Task<Result> Handle(TrainerToggleStatusCommand request, CancellationToken cancellationToken)
    {
        var trainer = await _unitOfWork.Repository<Trainer>()
          .FirstOrDefaultAsync(t => t.UserId == request.TrainerId, cancellationToken);

        if (trainer is null)
            return Result.Failure(UserErrors.UserNotFound);

        trainer.IsActive = !trainer.IsActive;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _memoryCache.Remove(_allTrainersCacheKey);
        _memoryCache.Remove(_activeTrainersCacheKey);
        return Result.Success();
    }
}
