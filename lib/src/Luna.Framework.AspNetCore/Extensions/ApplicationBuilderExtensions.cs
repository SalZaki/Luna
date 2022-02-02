using Luna.Framework.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Luna.Framework.AspNetCore.Extensions;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app)
  {
    if (app == null)
    {
      throw new ArgumentNullException(nameof(app));
    }

    return app.UseMiddleware<LoggingMiddleware>();
  }

  public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
  {
    if (app == null)
    {
      throw new ArgumentNullException(nameof(app));
    }

    return app.UseMiddleware<ExceptionHandlingMiddleware>();
  }

  public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app)
  {
    if (app == null)
    {
      throw new ArgumentNullException(nameof(app));
    }

    return app.UseMiddleware<SecurityHeadersMiddleware>();
  }

  public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder app)
  {
    if (app == null)
    {
      throw new ArgumentNullException(nameof(app));
    }

    return app.UseMiddleware<ApiKeyMiddleware>();
  }

  public static IApplicationBuilder UseLunaSwagger(
    this IApplicationBuilder app,
    IApiVersionDescriptionProvider provider = null)
  {
    var apiSettings = app.ApplicationServices.GetRequiredService<IOptions<ApiSettings>>().Value;

    if (!apiSettings.EnableSwagger)
    {
      return app;
    }

    app.UseSwagger();

    app.UseSwaggerUI(
      options =>
      {
        options.DocExpansion(DocExpansion.None);
        if (provider is null)
        {
          options.SwaggerEndpoint("/swagger/v1/swagger.json", apiSettings.Name);
        }
        else
        {
          foreach (var description in provider.ApiVersionDescriptions)
          {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
              description.GroupName.ToUpperInvariant());
          }
        }
      });

    return app;

  }
}

//
//
// public static WebApplicationBuilder AddCustomSwagger(
//   this WebApplicationBuilder builder,
//   IConfiguration configuration,
//   Assembly assembly)
// {
//   builder.Services.AddCustomSwagger(configuration, assembly);
//
//   return builder;
// }

//
// public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
// {
//   services.AddOptions<AppOptions>().Bind(configuration.GetSection(nameof(AppOptions)))
//     .ValidateDataAnnotations();
//
//   return services;
// }
//
// public static IServiceCollection AddCqrs(
//   this IServiceCollection services,
//   Assembly[] assemblies = null,
//   Action<IServiceCollection> doMoreActions = null)
// {
//   services.AddMediatR(assemblies ?? new[] { Assembly.GetExecutingAssembly() })
//     .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
//     .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
//     .AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>))
//     .AddScoped(typeof(IPipelineBehavior<,>), typeof(InvalidateCachingBehavior<,>));
//
//   services.AddTransient<ICommandProcessor, CommandProcessor>()
//     .AddTransient<IQueryProcessor, QueryProcessor>();
//
//   return services;
// }
