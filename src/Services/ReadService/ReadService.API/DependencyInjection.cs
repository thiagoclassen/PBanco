using BuildingBlocks.Behaviors;
using FluentValidation;
using MassTransit;
using ReadService.API.Features.ClientCreditCardView.Repository;
using ReadService.API.Features.Clients.Consumers;
using ReadService.API.Features.Clients.Repository;
using ReadService.API.Features.CreditCards.Consumers;
using ReadService.API.Features.CreditCards.Repository;
using ReadService.API.Features.Proposals.Consumers;
using ReadService.API.Features.Proposals.Repository;
using ReadService.API.Infrastructure;

namespace ReadService.API;

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
        services.AddMassTransitLib(services.BuildServiceProvider().GetRequiredService<IConfiguration>());
        // services.AddHangfire();

        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProposalRepository, ProposalRepository>();
        services.AddScoped<ICreditCardRepository, CreditCardRepository>();
        services.AddScoped<IClientCreditCardViewRepository, ClientCreditCardViewRepository>();

        return services;
    }

    private static void AddMassTransitLib(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumersFromNamespaceContaining<ProposalPropagateEventConsumer>();
            config.AddConsumersFromNamespaceContaining<ClientPropagateEventConsumer>();
            config.AddConsumersFromNamespaceContaining<CreditCardPropagateEventConsumer>();

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["EventBus:HostAddress"], h =>
                {
                    h.Username(configuration["EventBus:UserName"]!);
                    h.Password(configuration["EventBus:Password"]!);
                });

                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(
                    "read-service",
                    false
                ));

                cfg.UseMessageRetry(r => { r.Interval(3, TimeSpan.FromSeconds(10)); });

                cfg.UseJsonSerializer();
                cfg.UseJsonDeserializer();
            });
        });
    }

    // private static void AddHangfire(this IServiceCollection services)
    // {
    //     services.AddHangfire((serviceProvider, hangFireConfiguration) =>
    //     {
    //         var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    //         hangFireConfiguration.UseSqlServerStorage(GetConnectionString(configuration));
    //     });
    //
    //     services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1));
    //
    //     services.AddScoped<IProcessOutboxJob, ProcessOutboxJob>();
    // }

    private static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string is not configured.");

        return connectionString;
    }
}