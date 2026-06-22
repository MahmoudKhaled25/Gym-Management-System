using GymManagementSystem.Application.Exercises.Queries;
using GymManagementSystem.Application.ProgressLogs.Queries;
using GymManagementSystem.Application.WorkoutPlans.Queries;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using GymManagementSystem.Infrastructure.Persistence;
using GymManagementSystem.Infrastructure.Queries;
using GymManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagementSystem.Infrastructure;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
          throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        //services.AddControllers()
        // .AddJsonOptions(options =>
        // {
        //     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // });


        services.AddScoped<IRepository<Exercise>, Repository<Exercise>>();
        services.AddScoped<IExerciseQueries, ExerciseQueries>();
        services.AddScoped<IProgressLogQueries, ProgressLogQueries>();
        services.AddScoped<IWorkoutPlanQueries, WorkoutPlanQueries>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddMemoryCache();


        return services;
    }
}
