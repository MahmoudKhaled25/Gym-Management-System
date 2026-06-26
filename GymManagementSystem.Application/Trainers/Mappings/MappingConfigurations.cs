using GymManagementSystem.Application.Trainers.Dtos;
using GymManagementSystem.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Trainers.Mappings;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Trainer, TrainerDto>();
    }
}
