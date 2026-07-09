using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Consts;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Commands.AddAdmin;

public class AddAdminCommandHandler(IUnitOfWork unitOfWork,IIdentityService identityService) : IRequestHandler<AddAdminCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var admin = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender
            };
            var result = await _identityService.CreateAsync(admin, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Any(e => e.Code == "DuplicateEmail")
                    ? UserErrors.DuplicatedEmail
                    : UserErrors.InvalidCredentials;
                return Result.Failure(error);
            }
            var roleResult = await _identityService.AddToRoleAsync(admin, DefaultRoles.Admin.Name);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(UserErrors.InvalidRoles);
            }
            await transaction.CommitAsync(cancellationToken);
            return Result.Success();

        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure(UserErrors.InvalidCredentials);
        }
    }
}
