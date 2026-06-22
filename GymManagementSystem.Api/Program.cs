using GymManagementSystem.Application;
using GymManagementSystem.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSerilogRequestLogging();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
