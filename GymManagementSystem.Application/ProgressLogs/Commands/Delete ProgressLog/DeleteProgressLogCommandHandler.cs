using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Commands.Delete_ProgressLog;

public class DeleteProgressLogCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProgressLogCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteProgressLogCommand request, CancellationToken cancellationToken)
    {
       var progressLog = await _unitOfWork.Repository<ProgressLog>()
            .Query()
            .SingleOrDefaultAsync(x => x.Id == request.ProgressLogId && x.UserId == request.UserId);

        if (progressLog == null)
            return Result.Failure(ProgressLogErrors.ProgressLogNotFound);

         _unitOfWork.Repository<ProgressLog>()
            .Delete(progressLog);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
