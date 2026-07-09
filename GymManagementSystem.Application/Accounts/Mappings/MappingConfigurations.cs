using GymManagementSystem.Application.Accounts.Commands.UpdateUserProfile;
using GymManagementSystem.Application.Accounts.Dtos;
using GymManagementSystem.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Mappings;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateUserProfileCommand, ApplicationUser>()
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Ignore(dest => dest.Id);

        //// RegisterCommand → ApplicationUser
        //config.NewConfig<RegisterCommand, ApplicationUser>()
        //    .Map(dest => dest.UserName, src => src.Email);

        // ApplicationUser → UserProfileDto
        config.NewConfig<ApplicationUser, UserProfileDto>()
            .Map(dest => dest.Gender, src => src.Gender.ToString());
    }
}
