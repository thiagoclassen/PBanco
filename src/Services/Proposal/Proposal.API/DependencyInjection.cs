using BuildingBlocks.Behaviors;
using BuildingBlocks.Events.Client;
using BuildingBlocks.Outbox;
using BuildingBlocks.Outbox.Interceptor;
using BuildingBlocks.Outbox.Jobs;
using BuildingBlocks.Outbox.Persistence;
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
        
        services.AddScoped<UnitOfWork<ProposalDbContext>>();
        services.AddScoped<IProposalRepository, ProposalRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository<ProposalDbContext>>();
        
        return services;
    }
    
    private static void AddMassTransitLib(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit((config) =>
        {
            config.SetKebabCaseEndpointNameFormatter();
    
            config.AddConsumer<ClientCreatedConsumer>();
            
            
            
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["EventBus:HostAddress"], h =>
                {
                    h.Username(configuration["EventBus:UserName"]!);
                    h.Password(configuration["EventBus:Password"]!);
                });

                cfg.ConfigureEndpoints(ctx);
                
                cfg.UseMessageRetry(r =>
                {
                    r.Interval(3, TimeSpan.FromSeconds(10));
                });
                
                cfg.Message<ClientCreatedEvent>(x => x.SetEntityName("client-created"));
                
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
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var connectionString = configuration.GetConnectionString(env) ?? configuration[$"DatabaseStrings:{env}"];
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"Connection string for environment '{env}' is not configured.");

        return connectionString;
    }
}