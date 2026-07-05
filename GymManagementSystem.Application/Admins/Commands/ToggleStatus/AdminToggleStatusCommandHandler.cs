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
using static GymManagementSystem.Domain.Consts.DefaultRoles;

namespace GymManagementSystem.Application.Admins.Commands.ToggleStatus;

public class AdminToggleStatusCommandHandler(IUnitOfWork unitOfWork,IIdentityService identityService) : IRequestHandler<AdminToggleStatusCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(AdminToggleStatusCommand request, CancellationToken cancellationToken)
    {
        var admin = await _unitOfWork.Repository<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == request.AdminId, cancellationToken);
        if (admin == null)
        {
            return Result.Failure(UserErrors.UserNotFound);
        }
        if (!await _identityService.IsInRoleAsync(admin, DefaultRoles.Admin.Name))
        {
            return Result.Failure(UserErrors.UserNotFound);
        }

        admin.LockoutEnd = admin.LockoutEnd <= DateTimeOffset.UtcNow || admin.LockoutEnd == null
            ? DateTimeOffset.UtcNow.AddYears(100)
            : null;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
