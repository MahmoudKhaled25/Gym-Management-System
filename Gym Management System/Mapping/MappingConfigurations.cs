using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Auth;
using Gym_Management_System.Contracts.Member;
using GymManagementSystem.Contracts.WorkoutPlan;
using MapsterMapper;

namespace GymManagementSystem.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // ApplicationUser → UserProfileResponse
        config.NewConfig<ApplicationUser, UserProfileResponse>()
            .Map(dest => dest.Roles, src => new List<string>());

        // RegisterRequest → ApplicationUser
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);

        // AddMemberRequest → ApplicationUser
        config.NewConfig<AddMemberRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);

        // WorkoutPlanRequest → WorkoutPlan
        config.NewConfig<WorkoutPlanRequest, WorkoutPlan>()
            .Map(dest => dest.TrainerId, src => src.TrainerId)
            .Map(dest => dest.UserId, src => src.MemberId);
    }
}