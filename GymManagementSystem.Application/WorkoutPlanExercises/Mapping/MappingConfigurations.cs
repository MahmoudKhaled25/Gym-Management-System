using GymManagementSystem.Application.WorkoutPlanExercises.Commands.Add_WorkoutPlanExercise;
using GymManagementSystem.Domain.Entities;
using Mapster;

namespace GymManagementSystem.Application.WorkoutPlanExercises.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddWorkoutPlanExerciseCommand, WorkoutPlanExercise>();
    }
}
