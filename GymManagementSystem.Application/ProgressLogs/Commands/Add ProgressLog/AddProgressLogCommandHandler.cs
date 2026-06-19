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

namespace GymManagementSystem.Application.ProgressLogs.Commands.Add_ProgressLog;

public class AddProgressLogCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddProgressLogCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddProgressLogCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var progressLog = request.Adapt<ProgressLog>();
            progressLog.UserId = request.UserId;

            await _unitOfWork.Repository<ProgressLog>().AddAsync(progressLog);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error(ex.Source!, ex.Message,StatusCodes.Status400BadRequest));
        }
    }
}
