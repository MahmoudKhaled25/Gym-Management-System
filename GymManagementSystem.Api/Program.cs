using GymManagementSystem.Application;
using GymManagementSystem.Application.Interfaces;
using GymManagementSystem.Infrastructure;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();
//builder.Services.AddOpenApi();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}
app.UseSerilogRequestLogging();


app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = ""
});
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
