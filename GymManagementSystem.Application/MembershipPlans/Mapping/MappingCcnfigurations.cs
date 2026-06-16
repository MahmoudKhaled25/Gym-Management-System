using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Entities;
using Mapster;
namespace GymManagementSystem.Application.MembershipPlans.Mapping;

public class MappingCcnfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MembershipPlan,MembershipPlanDto>();
    }
}
