using BuildingBlocks.Outbox.Extensions;
using Clients.API;
using Clients.API.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Services container.

builder.Services
    .AddPresentation()
    .AddInfrastructure()
    .AddApplication();


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