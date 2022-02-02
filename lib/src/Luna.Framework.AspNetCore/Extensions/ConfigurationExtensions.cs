using Microsoft.Extensions.Configuration;

namespace Luna.Framework.AspNetCore.Extensions;

public static class ConfigurationExtensions
{
  public static ApiSettings GetApiSettings(this IConfiguration config)
  {
    var apiSettings = new ApiSettings();
    config.GetApiSettingsConfigSection()?.Bind(apiSettings);
    return apiSettings;
  }

  public static IConfigurationSection GetApiSettingsConfigSection(this IConfiguration config)
  {
    return config.GetSection(Constants.Configuration.ApiSettingsSection);
  }

  public static IConfigurationSection GetHttpClientSettingsConfigSection(this IConfiguration config)
  {
    return config.GetSection(Constants.Configuration.HttpClientSettingsSection);
  }
}
