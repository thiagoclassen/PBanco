using BuildingBlocks.Behaviors;
using Clients.API.Client.Persistence;
using Clients.API.Data;
using Clients.API.Data.Interceptors;
using Clients.API.Messages;
using Clients.API.Outbox.Job;
using Clients.API.Outbox.Persistence;
using FluentValidation;
using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Clients.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            options.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        return services;
    }
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<CreateOutboxMessagesInterceptor>();
        services.AddDbContext<ClientDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetRequiredService<CreateOutboxMessagesInterceptor>());

            //TODO - move connection string to configuration/env
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

        services.AddHangfire();
        services.AddMassTransitLib(services.BuildServiceProvider().GetRequiredService<IConfiguration>());
        
        services.AddScoped<UnitOfWork>();
        services.AddScoped<MessageService>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        
        return services;
    }

    private static IServiceCollection AddHangfire(this IServiceCollection services)
    {
        services.AddHangfire(configuration =>
        {
            configuration.UseSqlServerStorage(
                "Server=client.db,1433;Database=Bank;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=Yes");
        });

        services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1));

        services.AddScoped<IProcessOutboxJob, ProcessOutboxJob>();
        return services;
    } 
    
    private static IServiceCollection AddMassTransitLib(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();
    
            config.UsingRabbitMq((ctx, cfg) =>
            {
        
                cfg.Host(configuration["EventBus:HostAddress"], h =>
                {
                    h.Username(configuration["EventBus:UserName"]!);
                    h.Password(configuration["EventBus:Password"]!);
                });
        
                cfg.ConfigureEndpoints(ctx);
            });
        });
        
        return services;
    }
    
}