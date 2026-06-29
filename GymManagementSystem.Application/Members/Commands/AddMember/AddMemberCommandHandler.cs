using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Consts;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Members.Commands.AddMember;

public class AddMemberCommandHandler(IUnitOfWork unitOfWork,IIdentityService identityService) : IRequestHandler<AddMemberCommand, Result<AddMemberDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<AddMemberDto>> Handle(AddMemberCommand request, CancellationToken cancellationToken)
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
                DateOfBirth = request.DateOfBirth,
                Weight = request.Weight,
                Height = request.Height,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _identityService.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Any(e => e.Code == "DuplicateEmail")
                     ? UserErrors.DuplicatedEmail
                     : UserErrors.InvalidCredentials;
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure<AddMemberDto>(error);
            }
            var roleResult = await _identityService.AddToRoleAsync(user, DefaultRoles.Member.Name);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure<AddMemberDto>(UserErrors.InvalidRoles);
            }
            await transaction.CommitAsync(cancellationToken);
            var mappedResult = user.Adapt<AddMemberDto>();
            return Result.Success(mappedResult);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<AddMemberDto>(UserErrors.InvalidCredentials);
        }
    }
}
