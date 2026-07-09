using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Commands.Update_ProgressLog;

public class UpdateProgressLogCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateProgressLogCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateProgressLogCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var progressLog = await _unitOfWork.Repository<ProgressLog>()
                 .Query()
                 .FirstOrDefaultAsync(x => x.Id == request.ProgressLogId && x.UserId == request.UserId, cancellationToken);

            if (progressLog is null)
                return Result.Failure(ProgressLogErrors.ProgressLogNotFound);

         var entity = request.Adapt(progressLog);
             _unitOfWork.Repository<ProgressLog>().Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error(ex.Source!, ex.Message, StatusCodes.Status400BadRequest));
        }
    }
}
