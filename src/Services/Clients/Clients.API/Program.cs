using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Clients.API.Client;
using Clients.API.Client.Interfaces;
using Clients.API.Client.Persistence;
using Clients.API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Services container.


builder.Services.AddDbContext<ClientDbContext>(options =>
{
    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    {
        options.UseSqlServer(
            "Server=localhost,1433;Database=Bank;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=Yes");
    }

    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Container")
    {
        options.UseSqlServer(
            "Server=client.db,1433;Database=Bank;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=Yes");
    }
});

builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddMediatR(options =>
{
    options.AddOpenBehavior(typeof(ValidationBehavior<,>));
    options.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();
// HTTP request pipeline.

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ClientDbContext>();
context.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.MapControllers();

app.Run();