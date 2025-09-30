using BuildingBlocks.Behaviors;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProposalApi.Consumer;
using ProposalApi.Data;
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
        services.AddDbContext<ProposalDbContext>((serviceProvider, options) =>
        {
            //options.AddInterceptors(serviceProvider.GetRequiredService<CreateOutboxMessagesInterceptor>());

            //TODO - move connection string to configuration/env
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.UseSqlServer(
                    "Server=localhost,1443;Database=Proposals;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=Yes");
            }

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Container")
            {
                options.UseSqlServer(
                    "Server=proposal.db,1443;Database=Proposals;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=Yes");
            }
        });

        services.AddScoped<IProposalRepository, ProposalRepository>();
        
        services.AddMassTransitLib(services.BuildServiceProvider().GetRequiredService<IConfiguration>());
        
        return services;
    }
    
    private static IServiceCollection AddMassTransitLib(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
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
            });
        });
        
        return services;
    }
}