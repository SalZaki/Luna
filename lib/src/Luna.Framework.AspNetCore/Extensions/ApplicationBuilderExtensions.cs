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
