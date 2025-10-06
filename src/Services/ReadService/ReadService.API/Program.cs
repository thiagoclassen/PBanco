using ReadService.API;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseExceptionHandler("/error");
app.MapControllers();

app.Run();