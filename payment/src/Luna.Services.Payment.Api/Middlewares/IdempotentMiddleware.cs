using System.Text.Json;
using Luna.Framework.AspNetCore;
using Luna.Framework.AspNetCore.Dtos;
using Luna.Framework.AspNetCore.Resolvers;
using Luna.Services.Payment.Application.Dtos;
using Luna.Services.Payment.Application.Queries;
using MediatR;

namespace Luna.Services.Payment.Api.Middlewares;

public sealed class IdempotentMiddleware
{
  private readonly RequestDelegate _next;

  private readonly IIdempotentKeyResolver _idempotentKeyResolver;

  private readonly ILogger _logger;

  public IdempotentMiddleware(
    RequestDelegate next,
    IIdempotentKeyResolver idempotentKeyResolver,
    ILoggerFactory loggerFactory)
  {
    _next = next;
    _idempotentKeyResolver = idempotentKeyResolver;
    _logger = loggerFactory.CreateLogger<IdempotentMiddleware>();
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // We are only interested in post method
    var idempotentKeyExists = context?.Request?.Method?.ToUpper() == "POST" &&
                              context.Request.Headers.ContainsKey(Constants.RequestHeaderKeys.IdempotentKey);
    if (idempotentKeyExists)
    {
      var mediator = context?.RequestServices.GetRequiredService<IMediator>();
      var id = _idempotentKeyResolver.GetIdempotentKey();

      var cancellationToken = context?.RequestAborted ?? CancellationToken.None;
      var response = await mediator.Send(new GetPaymentByIdempotentKeyQuery { IdempotentKey = id }, cancellationToken);

      if (response != null)
      {
        await WriteResponseAsync(context, response, cancellationToken);

        return;
      }
    }

    if (context != null) await _next.Invoke(context);
  }

  private static async Task WriteResponseAsync(HttpContext? context, PaymentDto response,
    CancellationToken cancellationToken)
  {
    if (context != null)
    {
      var responseBody = JsonSerializer.Serialize(CreateSuccessResponse(response));
      context.Response.StatusCode = 200;
      context.Response.Headers.Add("content-type", Constants.MimeTypes.ApplicationJson);
      context.Request.ContentLength = responseBody.Length;
      await context.Response.WriteAsync(responseBody, cancellationToken);
    }
  }

  private static ResponseDto<PaymentDto> CreateSuccessResponse(PaymentDto response)
  {
    return new ResponseDto<PaymentDto>
    {
      Data = response,
      Status = "Success",
      Version = "1.0"
    };
  }
}
