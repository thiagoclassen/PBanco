using BuildingBlocks.Outbox.Extensions;
using CreditCard.API;
using CreditCard.API.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();
// Configure the HTTP request pipeline.

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<CreditCardDbContext>();
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