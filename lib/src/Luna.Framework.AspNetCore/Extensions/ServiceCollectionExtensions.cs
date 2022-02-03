using System.Text.RegularExpressions;
using Luna.Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Luna.Framework.AspNetCore.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApiVersionCore(this IServiceCollection services, IConfiguration config)
  {
    var apiSettings = config.GetApiSettings();

    services.AddRouting(o => o.LowercaseUrls = true);
    services.AddVersionedApiExplorer(
      o =>
      {
        o.GroupNameFormat = "'v'VVV";
        o.SubstituteApiVersionInUrl = true;
      });
    services.AddApiVersioning(
      o =>
      {
        o.ReportApiVersions = true;
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = ParseApiVersion(apiSettings.Version);
      });

    return services;
  }

  public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration config)
  {
    var apiSettings = config.GetApiSettings();

    services.AddSwaggerGen(c =>
    {
      var provider = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
      foreach (var description in provider.ApiVersionDescriptions)
      {
        c.SwaggerDoc(
          description.GroupName,
          CreateInfoForApiVersion(description, apiSettings));
      }

      c.OperationFilter<IdempotentKeyFilter>();
      c.EnableAnnotations();
      c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
      c.AddSecurityDefinition(Constants.RequestHeaderKeys.ApiKey, new OpenApiSecurityScheme
      {
        Name = Constants.RequestHeaderKeys.ApiKey,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description =
          "Publicly known api key defined per Luna client and per Luna environment, i.e dev, sand-box or prd"
      });

      c.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme
          {
            Name = Constants.RequestHeaderKeys.ApiKey,
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = Constants.RequestHeaderKeys.ApiKey
            }
          },
          new[] {"NXyRtwK27y66shI"}
        }
      });
    });

    return services;
  }

  private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, ApiSettings apiSettings)
  {
    var info = new OpenApiInfo
    {
      Title = $"{apiSettings.Title} {description.ApiVersion}",
      Version = description.ApiVersion.ToString(),
      Description = apiSettings.Description,
      TermsOfService = apiSettings.TermOfServiceUrl,
      Contact = new OpenApiContact
      {
        Name = apiSettings.ContactName,
        Email = apiSettings.ContactEmail,
      },
      License = new OpenApiLicense
      {
        Name = apiSettings.LicenseName,
        Url = apiSettings.LicenseUrl
      }
    };

    if (description.IsDeprecated)
    {
      info.Description += " This API version has been deprecated.";
    }

    return info;
  }

  public static IServiceCollection AddApiSettings(this IServiceCollection services, IConfiguration config)
  {
    services
      .AddOptions<ApiSettings>()
      .Bind(config.GetApiSettingsConfigSection())
      .ValidateDataAnnotations();

    return services;
  }

  public static IServiceCollection AddHttpClientSettings(this IServiceCollection services, IConfiguration config)
  {
    services
      .AddOptions<HttpClientSettings>()
      .Bind(config.GetHttpClientSettingsConfigSection())
      .ValidateDataAnnotations();

    return services;
  }

  private static ApiVersion ParseApiVersion(string? apiVersion)
  {
    if (string.IsNullOrEmpty(apiVersion))
    {
      throw new FrameworkException("ApiVersion version is null or empty.");
    }

    const string VersionPattern = @"(.)|(-)";

    var results = Regex
      .Split(apiVersion, VersionPattern)
      .Where(x => x != string.Empty && x != "." && x != "-")
      .ToArray();

    if (results == null || results.Count() < 2)
    {
      throw new FrameworkException("Could not parse ServiceVersion.");
    }

    if (results.Length > 2)
    {
      return new ApiVersion(Convert.ToInt32(results[0]), Convert.ToInt32(results[1]), results[2]);
    }

    return new ApiVersion(Convert.ToInt32(results[0]), Convert.ToInt32(results[1]));
  }
}
