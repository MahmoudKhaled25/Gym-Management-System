using Gym_Management_System;
using Gym_Management_System.Persistence;
using GymManagementSystem.Seeders;
using GymManagementSystem.Services;
using Hangfire;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDependencies(builder.Configuration);


var app = builder.Build();
//using var scope = app.Services.CreateScope();
//var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//await ProgressLogSeeder.SeedProgressLogsAsync(context);
//await WorkoutPlanExercisesSeeder.SeedWorkoutPlanExercisesAsync(context);
////await WorkoutPlanSeeder.SeedWorkoutPlansAsync(context);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization = [
        new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
            Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
        }
    ],
    DashboardTitle = "Gym Management Dashboard"
});

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
var subscriptionJobService = scope.ServiceProvider.GetRequiredService<ISubscriptionJobService>();

RecurringJob.AddOrUpdate(
    "expire-subscriptions",
    () => subscriptionJobService.ExpireSubscriptionsAsync(),
    Cron.Daily());

RecurringJob.AddOrUpdate(
    "notify-expiring-subscriptions",
    () => subscriptionJobService.NotifyExpiringSubscriptionsAsync(),
     Cron.Daily());


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();
