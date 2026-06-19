using GymManagementSystem.Application.ProgressLogs.Commands.Add_ProgressLog;
using GymManagementSystem.Application.ProgressLogs.Dtos;
using GymManagementSystem.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Mappings;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddProgressLogCommand,ProgressLog>();
        config.NewConfig<ProgressLog, AllProgressLogsDto>()
              .Map(dest => dest.MemberFullName,
                   src => src.User != null
                          ? $"{src.User.FirstName} {src.User.LastName}".Trim()
                          : string.Empty);

        config.NewConfig<ProgressLog, MyProgressLogsDto>();
    }
}
