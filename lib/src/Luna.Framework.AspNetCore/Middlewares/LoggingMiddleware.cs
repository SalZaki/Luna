using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Luna.Framework.AspNetCore.Middlewares;

internal sealed class LoggingMiddleware
{
  private readonly RequestDelegate _next;

  private readonly ILogger _logger;

  public LoggingMiddleware(
    RequestDelegate next,
    ILoggerFactory loggerFactory)
  {
    _next = next;
    _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
  }

  public async Task InvokeAsync(HttpContext context)
  {
    _logger.Log(LogLevel.Information,$"Started executing the request with ID: {context.TraceIdentifier}");

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    await _next(context);

    stopwatch.Stop();
    var time = stopwatch.ElapsedMilliseconds;

    _logger.Log(LogLevel.Information,$"Finished executing the request with ID: {context.TraceIdentifier} in {time} ms.");
  }
}
