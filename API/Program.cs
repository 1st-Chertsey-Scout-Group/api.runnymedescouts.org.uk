
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RunnymedeScouts.API.Extensions;
using RunnymedeScouts.API.Options;
using RunnymedeScouts.API.Services;

namespace RunnymedeScouts.API;

public static class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var services = builder.Services;
        try
        {
            configuration.AddEnvironmentVariables();

            services
                .AddControllers()
                    .ConfigureApiBehavior();

            services
                .AddRateLimiter()
                .AddOptions(configuration)
                .AddServices();

            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
        }
        catch (System.Exception)
        {

        }
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseCors();

        app.MapControllers();



        app.Run();
    }


    public static IMvcBuilder ConfigureApiBehavior(this IMvcBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                return new BadRequestObjectResult(actionContext.ModelState.FriendlyMessage());
            };
        });

        return builder;
    }

    public static IServiceCollection AddRateLimiter(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddRateLimiter(_ => _
            .AddFixedWindowLimiter(policyName: "Fixed", options =>
            {
                options.PermitLimit = 1;
                options.Window = TimeSpan.FromSeconds(10);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 2;
            }));

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<ISmtpService, SmtpService>();
        services.AddSingleton<IEmailTemplateService, EmailTemplateService>();

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);


        services
            .AddOptionsWithValidateOnStart<SmtpOptions>()
            .BindConfiguration("Smtp")
            .ValidateDataAnnotations();

        services
            .AddOptionsWithValidateOnStart<CorsOptions>()
            .BindConfiguration("Cors")
            .ValidateDataAnnotations();

        return services;
    }

    public static IApplicationBuilder UseCors(this IApplicationBuilder app, ConfigurationManager configuration)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(configuration);

        var corsOptions = configuration.GetRequiredSection("Cors").Get<CorsOptions>();
        if (corsOptions != null)
        {
            app.UseCors(x => x.WithOrigins(corsOptions.AllowedOriginsList)
                              .WithHeaders(corsOptions.AllowedHeadersList)
                              .WithMethods(corsOptions.AllowedMethodsList)
                              .AllowCredentials());
        }

        return app;
    }
}
