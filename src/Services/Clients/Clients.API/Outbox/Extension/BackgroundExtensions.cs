using Clients.API.Outbox.Job;
using Hangfire;

namespace Clients.API.Outbox.Extension;

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