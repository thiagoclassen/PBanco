using Clients.API;
using Clients.API.Data;
using Clients.API.Messages;
using Clients.API.Outbox.Extension;
using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Services container.

builder.Services
    .AddPresentation()
    .AddInfrastructure()
    .AddApplication();

builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();
    
    config.UsingRabbitMq((ctx, cfg) =>
    {
        
        cfg.Host(builder.Configuration["EventBus:HostAddress"], h =>
        {
            h.Username(builder.Configuration["EventBus:UserName"]!);
            h.Password(builder.Configuration["EventBus:Password"]!);
        });
        
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddScoped<MessageService>();

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

app.UseHangfireDashboard(options: new DashboardOptions { Authorization = [], DarkModeEnabled = true });
app.UseBackgroundJobs();

app.UseExceptionHandler("/error");

app.MapControllers();

app.Run();