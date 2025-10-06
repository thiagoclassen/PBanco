using BuildingBlocks.Behaviors;
using BuildingBlocks.Outbox.Interceptor;
using BuildingBlocks.Outbox.Jobs;
using BuildingBlocks.Outbox.Persistence;
using BuildingBlocks.ProcessedEvents.Filter;
using BuildingBlocks.ProcessedEvents.Persistence;
using BuildingBlocks.UnitOfWork;
using CreditCard.API.Consumer;
using CreditCard.API.CreditCard.Persistence;
using CreditCard.API.Data;
using CreditCard.API.Outbox.Jobs;
using FluentValidation;
using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CreditCard.API;

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
        services.AddDbContext<CreditCardDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetRequiredService<CreateOutboxMessagesInterceptor>());

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            options.UseSqlServer(GetConnectionString(configuration));
        });

        services.AddHangfire();
        services.AddMassTransitLib(services.BuildServiceProvider().GetRequiredService<IConfiguration>());

        services.AddScoped<IUnitOfWork, UnitOfWork<CreditCardDbContext>>();
        services.AddScoped<ICreditCardRepository, CreditCardRepository>();
        services.AddScoped<IProcessedEventsRepository, ProcessedEventsRepository<CreditCardDbContext>>();
        services.AddScoped<IOutboxRepository, OutboxRepository<CreditCardDbContext>>();

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

            config.AddConsumersFromNamespaceContaining<ProposalApprovedConsumer>();

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["EventBus:HostAddress"], h =>
                {
                    h.Username(configuration["EventBus:UserName"]!);
                    h.Password(configuration["EventBus:Password"]!);
                });

                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(
                    "credit-card-service",
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
            throw new InvalidOperationException("Connection string is not configured.");

        return connectionString;
    }
}