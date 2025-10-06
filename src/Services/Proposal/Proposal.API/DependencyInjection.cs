using BuildingBlocks.Behaviors;
using BuildingBlocks.Outbox.Interceptor;
using BuildingBlocks.Outbox.Jobs;
using BuildingBlocks.Outbox.Persistence;
using BuildingBlocks.ProcessedEvents.Filter;
using BuildingBlocks.ProcessedEvents.Persistence;
using BuildingBlocks.UnitOfWork;
using FluentValidation;
using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProposalApi.Consumer;
using ProposalApi.Data;
using ProposalApi.Outbox.Jobs;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
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
        services.AddDbContext<ProposalDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetRequiredService<CreateOutboxMessagesInterceptor>());

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            options.UseSqlServer(GetConnectionString(configuration));
        });

        services.AddMassTransitLib(services.BuildServiceProvider().GetRequiredService<IConfiguration>());
        services.AddHangfire();

        services.AddScoped<IUnitOfWork, UnitOfWork<ProposalDbContext>>();
        services.AddScoped<IProposalRepository, ProposalRepository>();
        services.AddScoped<IProcessedEventsRepository, ProcessedEventsRepository<ProposalDbContext>>();
        services.AddScoped<IOutboxRepository, OutboxRepository<ProposalDbContext>>();

        return services;
    }

    private static void AddMassTransitLib(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumersFromNamespaceContaining<ClientCreatedConsumer>();

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["EventBus:HostAddress"], h =>
                {
                    h.Username(configuration["EventBus:UserName"]!);
                    h.Password(configuration["EventBus:Password"]!);
                });

                cfg.UseConsumeFilter(typeof(ProcessedEventFilter<>), ctx);

                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(
                    "proposal-service",
                    false
                ));

                cfg.UseMessageRetry(r => { r.Interval(3, TimeSpan.FromSeconds(10)); });

                cfg.UseJsonSerializer();
                cfg.UseJsonDeserializer();
            });
        });
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

    private static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string is not configured.");

        return connectionString;
    }
}