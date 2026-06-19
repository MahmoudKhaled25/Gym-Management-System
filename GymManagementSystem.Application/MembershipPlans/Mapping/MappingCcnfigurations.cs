using GymManagementSystem.Application.MembershipPlans.Commands.Add_Plan;
using GymManagementSystem.Application.MembershipPlans.Dtos;
using GymManagementSystem.Domain.Entities;
using Mapster;
namespace GymManagementSystem.Application.MembershipPlans.Mapping;

public class MappingCcnfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MembershipPlan,MembershipPlanDto>();
        config.NewConfig<AddPlansCommand,MembershipPlan>();
    }
}
