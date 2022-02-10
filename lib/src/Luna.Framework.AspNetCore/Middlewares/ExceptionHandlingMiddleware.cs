using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using FluentValidation;
using Luna.Framework.AspNetCore.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;

namespace Luna.Framework.AspNetCore.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
  private readonly ApiSettings _apiSettings;

  private readonly RequestDelegate _next;

  private readonly ILogger _logger;

  private readonly IHostingEnvironment _hostingEnvironment;

  public ExceptionHandlingMiddleware(
    IOptions<ApiSettings> apiSettingsOptions,
    IHostingEnvironment hostingEnvironment,
    RequestDelegate next,
    ILoggerFactory loggerFactory)
  {
    _apiSettings = apiSettingsOptions.Value;
    _hostingEnvironment = hostingEnvironment;
    _next = next;
    _logger = loggerFactory.CreateLogger<ExceptionHandlingMiddleware>();
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next.Invoke(context);
    }
    catch (Exception ex)
    {
      _logger.Log(LogLevel.Error, ex, $"Exception: {ex.Message}");
      await HandleExceptionAsync(context, ex);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception ex)
  {
    var routeData = context.GetRouteData();
    if(routeData == null) return;
    var controllerName = routeData.Values["controller"]?.ToString().ToLowerInvariant();
    var problemDetails = CreateProblemDetails(ex, controllerName);

    var response = context.Response;
    response.ContentType = Constants.MimeTypes.ApplicationProblemJson;
    if (problemDetails.Status != null) response.StatusCode = problemDetails.Status.Value;
    await response.WriteAsync(JsonSerializer.Serialize(problemDetails));
  }

  private LunaProblemDetails CreateProblemDetails(Exception ex, string? controllerName)
  {
    LunaProblemDetails problemDetails;

    switch (ex)
    {
      case UnauthorizedAccessException unauthorizedAccessException:
        problemDetails = new LunaProblemDetails
        {
          Title = Constants.Errors.UnauthorizedAccess.ErrorTitle,
          Detail = Constants.Errors.UnauthorizedAccess.ErrorMessage + unauthorizedAccessException.Message,
          Status = StatusCodes.Status401Unauthorized,
          DocumentationUrl = _apiSettings.DocumentationUrl +
                             $"{controllerName}/{Constants.Errors.UnauthorizedAccess.ErrorCode}",
          StackTrace = unauthorizedAccessException.StackTrace
        };
        break;

      case ValidationException validationException:
        problemDetails = new LunaProblemDetails
        {
          Title = Constants.Errors.Validation.ErrorTitle,
          Detail = Constants.Errors.Validation.ErrorMessage + validationException.Message,
          Status = StatusCodes.Status400BadRequest,
          DocumentationUrl = _apiSettings.DocumentationUrl +
                             $"{controllerName}/{Constants.Errors.Validation.ErrorCode}",
          StackTrace = validationException.StackTrace
        };
        break;

      default:
        problemDetails = new LunaProblemDetails
        {
          Title = Constants.Errors.Server.ErrorTitle,
          Detail = Constants.Errors.Server.ErrorMessage,
          Status = StatusCodes.Status500InternalServerError,
          DocumentationUrl = _apiSettings.DocumentationUrl + $"{controllerName}/{Constants.Errors.Server.ErrorCode}",
          StackTrace = ex.StackTrace
        };
        break;
    }

    // No need to leak stack trace on PRD.
    if (_hostingEnvironment.IsProduction())
    {
      problemDetails.StackTrace = string.Empty;
    }

    return problemDetails;
  }
}
