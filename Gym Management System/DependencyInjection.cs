using FluentValidation;
using FluentValidation.AspNetCore;
using Gym_Management_System.Entities;
using Gym_Management_System.Persistence;
using Mapster;
using MapsterMapper;
using System.Reflection;

namespace Gym_Management_System;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services,IConfiguration configuration)
 {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(connectionString));

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddMapsterConfig()
            .AddFluentValidationConfig();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Lockout.MaxFailedAccessAttempts = 5;
            //options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail = true;
            //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(180);
        }
        );

        return services;
      }

    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        // add mappster
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        return services;
    }
    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;

    }


}
