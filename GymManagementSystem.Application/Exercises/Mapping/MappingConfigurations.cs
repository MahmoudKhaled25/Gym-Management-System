using GymManagementSystem.Application.Exercises.Commands.Add_Exercise;
using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Application.Exercises.Queries.Get_All_Exercises;
using GymManagementSystem.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Exercises.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddExerciseCommand, Exercise>();
        config.NewConfig<Exercise, ExerciseDto>();
    }
}
