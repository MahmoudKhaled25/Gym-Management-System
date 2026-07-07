using GymManagementSystem.Application.Accounts.Queries;
using GymManagementSystem.Application.Admins.Queries;
using GymManagementSystem.Application.Auth.Abstractions;
using GymManagementSystem.Application.Exercises.Queries;
using GymManagementSystem.Application.Interfaces;
using GymManagementSystem.Application.Members.Queries;
using GymManagementSystem.Application.ProgressLogs.Queries;
using GymManagementSystem.Application.SubscriptionRequests.Queries;
using GymManagementSystem.Application.Subscriptions.Queries;
using GymManagementSystem.Application.Trainers.Queries;
using GymManagementSystem.Application.WorkoutPlans.Queries;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using GymManagementSystem.Infrastructure.Authentication;
using GymManagementSystem.Infrastructure.Persistence;
using GymManagementSystem.Infrastructure.Queries;
using GymManagementSystem.Infrastructure.Repositories;
using GymManagementSystem.Infrastructure.Services;
using GymManagementSystem.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace GymManagementSystem.Infrastructure;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAuthConfig(configuration);

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
          throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        //services.AddControllers()
        // .AddJsonOptions(options =>
        // {
        //     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // });

        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(allowedOrigins)
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        services.AddScoped<IAccountQueries, AccountQueries>();
        services.AddScoped<IAdminQueries, AdminQueries>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IRepository<Exercise>, Repository<Exercise>>();
        services.AddScoped<IExerciseQueries, ExerciseQueries>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IMemberQueries, MemberQueries>();
        services.AddScoped<IProgressLogQueries, ProgressLogQueries>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ISubscriptionQueries, SubscriptionQueries>();
        services.AddScoped<ISubscriptionRequestQueries, SubscriptionRequestQueries>();
        services.AddScoped<ITrainerQueries, TrainerQueries>();
        services.AddScoped<IWorkoutPlanQueries, WorkoutPlanQueries>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        services.AddOptions<TwilioSettings>()
    .BindConfiguration(nameof(TwilioSettings))
    .ValidateDataAnnotations()
    .ValidateOnStart();

        services.AddOptions<EmailSettings>()
           .BindConfiguration(nameof(EmailSettings))
           .ValidateDataAnnotations()
           .ValidateOnStart();

        services.AddRateLimitingConfig();
        services.AddMemoryCache();



        return services;
    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddOptions<JwtOptions>()
           .BindConfiguration(JwtOptions.SectionName)
           .ValidateDataAnnotations()
           .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ??
            throw new InvalidOperationException($"Failed to bind JWT settings from configuration section '{JwtOptions.SectionName}'.");


        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience
            };
        });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);

        }
       );

        return services;
    }
    private static IServiceCollection AddRateLimitingConfig(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {

            // Auth Endpoints
            options.AddPolicy("AuthByIp", context =>
            RateLimitPartition.GetFixedWindowLimiter(
             partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
         factory: _ => new FixedWindowRateLimiterOptions
         {
             Window = TimeSpan.FromMinutes(1),
             PermitLimit = 5,
             QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
             QueueLimit = 0
         }));

            // Account & Profile Endpoints
            options.AddFixedWindowLimiter("Account", limiterOptions =>
            {
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.PermitLimit = 20;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
            });

            // General Endpoints
            options.AddSlidingWindowLimiter("General", limiterOptions =>
            {
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.PermitLimit = 30;
                limiterOptions.SegmentsPerWindow = 6;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }
}
