using System.Text.RegularExpressions;
using Luna.Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
