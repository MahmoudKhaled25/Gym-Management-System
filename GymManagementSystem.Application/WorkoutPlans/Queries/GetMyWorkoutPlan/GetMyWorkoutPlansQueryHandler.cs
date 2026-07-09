using GymManagementSystem.Application.WorkoutPlans.Dtos;
using GymManagementSystem.Application.WorkoutPlans.Queries.GetMyWorkoutPlans;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Queries.GetMyWorkoutPlan;
    
    public class GetMyWorkoutPlansQueryHandler(IUnitOfWork unitOfWork): IRequestHandler<GetMyWorkoutPlansQuery, Result<IEnumerable<WorkoutPlanGroupedDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<IEnumerable<WorkoutPlanGroupedDto>>> Handle(GetMyWorkoutPlansQuery request,CancellationToken cancellationToken)
        {
            var plans = await _unitOfWork.Repository<WorkoutPlan>()
                .Query()
                .Where(x => x.UserId == request.UserId)
                .Select(x => new
                {
                    TrainerName = x.Trainer == null
                        ? null
                        : $"{x.Trainer.ApplicationUser!.FirstName} {x.Trainer.ApplicationUser.LastName}",

                    Plan = new MyWorkoutPlanDto(
                        x.Id,
                        x.Name,
                        x.Description
                    )
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var result = plans
                .GroupBy(x => x.TrainerName)
                .Select(g => new WorkoutPlanGroupedDto(
                    g.Key,
                    g.Select(x => x.Plan)
                ));

            return Result.Success(result);
        }
    }
