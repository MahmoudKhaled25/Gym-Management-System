using GymManagementSystem.Application.Members.Dtos;
using GymManagementSystem.Domain.Entities;
using Mapster;

namespace GymManagementSystem.Application.Members.Mappings;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, AddMemberDto>();
            

    }
}
