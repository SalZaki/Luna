using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Luna.Framework.AspNetCore.Middlewares;

internal sealed class ApiKeyMiddleware
{
  private readonly RequestDelegate _next;

  private readonly ILogger _logger;

  private readonly string? _apiKey;

  public ApiKeyMiddleware(
    IOptions<ApiSettings> apiSettingsOptions,
    RequestDelegate next,
    ILoggerFactory loggerFactory)
  {
    if (string.IsNullOrWhiteSpace(apiSettingsOptions?.Value?.ApiKey))
    {
      throw new InvalidOperationException("API Key is null or empty string");
    }

    _apiKey = apiSettingsOptions?.Value?.ApiKey;

    _next = next;

    _logger = loggerFactory.CreateLogger<ApiKeyMiddleware>();
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var isAuthorised = false;

    var key = context?.Request?.Headers[Constants.RequestHeaderKeys.ApiKey].FirstOrDefault();

    isAuthorised = key == _apiKey;

    if (isAuthorised)
    {
      await _next(context);
    }
    else
    {
      throw new UnauthorizedAccessException("Api key does not exist or is invalid in request header.");
    }
  }
}
