using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Luna.Framework.AspNetCore.Middlewares;

internal sealed class SecurityHeadersMiddleware
{
  private readonly RequestDelegate _next;

  private readonly ILogger _logger;

  public SecurityHeadersMiddleware(
    RequestDelegate next,
    ILoggerFactory loggerFactory)
  {
    _next = next;
    _logger = loggerFactory.CreateLogger<SecurityHeadersMiddleware>();
  }

  public async Task InvokeAsync(HttpContext context)
  {
    _logger.Log(LogLevel.Information, $"Started adding security headers to response");

    context.Response.Headers.Add("x-frame-options", new StringValues("DENY"));
    context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));
    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));
    context.Response.Headers.Add("x-xss-protection", new StringValues("0"));

    _logger.Log(LogLevel.Information, $"Finished adding security headers to response");

    await _next(context);
  }
}
