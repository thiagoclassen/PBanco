using Microsoft.EntityFrameworkCore;
using ProposalApi;
using ProposalApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure()
    .AddPresentation()
    .AddApplication();



var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ProposalDbContext>();
context.Database.Migrate();


app.UseExceptionHandler("/error");

app.MapControllers();


app.Run();