using BuildingBlocks.Outbox.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Outbox.Extensions;

public static class BackgroundExtensions 
{
    public static IApplicationBuilder UseBackgroundJobs(this WebApplication app)
    {
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IProcessOutboxJob>("outbox-processor",
                job => job.ProcessAsync(CancellationToken.None),
                app.Configuration["BackgroundJobs:Outbox:Schedule"]);
        
        return app;
    }
}