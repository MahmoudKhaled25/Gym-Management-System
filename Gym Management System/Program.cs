using Gym_Management_System;
using Gym_Management_System.Persistence;
using GymManagementSystem.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();
