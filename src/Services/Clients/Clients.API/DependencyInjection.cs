using BuildingBlocks.Behaviors;
using BuildingBlocks.Events.Client;
using BuildingBlocks.Outbox;
using BuildingBlocks.Outbox.Interceptor;
using BuildingBlocks.Outbox.Persistence;
using BuildingBlocks.ProcessedEvents.Filter;
using BuildingBlocks.ProcessedEvents.Persistence;
using BuildingBlocks.UnitOfWork;
using Clients.API.Client.Persistence;
using Clients.API.Data;
using Clients.API.Outbox.Jobs;
using FluentValidation;
using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using IProcessOutboxJob = BuildingBlocks.Outbox.Jobs.IProcessOutboxJob;

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

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            options.UseSqlServer(GetConnectionString(configuration));
        });

        services.AddHangfire();
        services.AddMassTransitLib(services.BuildServiceProvider().GetRequiredService<IConfiguration>());

        services.AddScoped<IUnitOfWork, UnitOfWork<ClientDbContext>>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProcessedEventsRepository, ProcessedEventsRepository<ClientDbContext>>();
        services.AddScoped<IOutboxRepository, OutboxRepository<ClientDbContext>>();

        return services;
    }

    private static void AddHangfire(this IServiceCollection services)
    {
        services.AddHangfire((serviceProvider, hangFireConfiguration) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            hangFireConfiguration.UseSqlServerStorage(GetConnectionString(configuration));
        });

        services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1));

        services.AddScoped<IProcessOutboxJob, ProcessOutboxJob>();
    }

    private static void AddMassTransitLib(this IServiceCollection services, IConfiguration configuration)
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
                
                cfg.UseMessageRetry(r => r.Immediate(3));
                cfg.UseMessageRetry(r => r.Interval(1, TimeSpan.FromMinutes(1)));

                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(
                    prefix: "credit-card-service",
                    false
                ));

                cfg.UseConsumeFilter(typeof(ProcessedEventFilter<>), ctx);
                
                cfg.UseJsonSerializer();
                cfg.UseJsonDeserializer();
            });
        });
    }
    
    private static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"Connection string is not configured.");

        return connectionString;
    }
}