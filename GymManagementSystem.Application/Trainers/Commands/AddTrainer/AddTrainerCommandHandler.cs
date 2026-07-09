using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using GymManagementSystem.Domain.Consts;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace GymManagementSystem.Application.Trainers.Commands.AddTrainer;

public class AddTrainerCommandHandler(IUnitOfWork unitOfWork,IIdentityService identityService,IMemoryCache memoryCache) : IRequestHandler<AddTrainerCommand, Result<TrainerDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdentityService _identityService = identityService;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _allTrainersCacheKey = "Trainers_All";
    private const string _activeTrainersCacheKey = "Trainers_Active";


    public async Task<Result<TrainerDto>> Handle(AddTrainerCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true,
                NormalizedEmail = request.Email.ToUpper(),
                NormalizedUserName = request.Email.ToUpper()
            };
            var result = await _identityService.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Any(e => e.Code == "DuplicateEmail")
                            ? UserErrors.DuplicatedEmail
                            : UserErrors.InvalidCredentials;
                return Result.Failure<TrainerDto>(error);
            }
            var roleResult = await _identityService.AddToRoleAsync(user, DefaultRoles.Trainer.Name);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure<TrainerDto>(UserErrors.InvalidRoles);

            }
            var trainer = new Trainer
            {
                UserId = user.Id,
                Specialization = request.Specialization,
                IsActive = true
            };

            await _unitOfWork.Repository<Trainer>().AddAsync(trainer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var response = new TrainerDto(
                user.Id,
                user.FirstName,
                user.LastName,
                trainer.Specialization,
                trainer.IsActive,
                new List<string> { DefaultRoles.Trainer.Name }
            );
            _memoryCache.Remove(_allTrainersCacheKey);
            _memoryCache.Remove(_activeTrainersCacheKey);
            return Result.Success(response);

        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<TrainerDto>(UserErrors.UpdateFailed);

        }
    }
    }

