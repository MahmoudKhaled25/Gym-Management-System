using GymManagementSystem.Application.WorkoutPlans.Commands.AddWorkoutPlan;
using GymManagementSystem.Application.WorkoutPlans.Commands.UpdateWorkoutPlan;
using GymManagementSystem.Application.WorkoutPlans.Dtos;
using GymManagementSystem.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<WorkoutPlan,WorkoutPlanDto>();
        config.NewConfig<AddWorkoutPlanCommand, WorkoutPlan>()
          .Map(dest => dest.UserId, src => src.MemberId);
        config.NewConfig<UpdateWorkoutPlanCommand, WorkoutPlan>()
         .Map(dest => dest.UserId, src => src.MemberId);
    }
}
