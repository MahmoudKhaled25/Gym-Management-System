using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using static GymManagementSystem.Domain.Consts.DefaultRoles;

namespace GymManagementSystem.Application.Trainers.Commands.UpdateTrainer;

public class UpdateTrainerCommandHandler(IUnitOfWork unitOfWork, IIdentityService identityService, IMemoryCache memoryCache) : IRequestHandler<UpdateTrainerCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdentityService _identityService = identityService;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _allTrainersCacheKey = "Trainers_All";
    private const string _activeTrainersCacheKey = "Trainers_Active";
    public async Task<Result> Handle(UpdateTrainerCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserWithTrainerAsync(request.TrainerId);

        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        request.Adapt(user);

        if (user.Trainer is not null)
        {
            user.Trainer.Specialization = request.Specialization;
        }

        var result = await _identityService.UpdateAsync(user);

        if (!result.Succeeded)
            return Result.Failure(UserErrors.UpdateFailed);

        _memoryCache.Remove(_allTrainersCacheKey);
        _memoryCache.Remove(_activeTrainersCacheKey);

        return Result.Success();
    }
}
