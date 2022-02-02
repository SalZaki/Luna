using Luna.Framework.AspNetCore;
using Luna.Framework.AspNetCore.Controllers;
using Luna.Framework.AspNetCore.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using Luna.Framework.AspNetCore.Dtos;
using Luna.Framework.AspNetCore.Resolvers;
using Luna.Services.Payment.Application.Commands;
using Luna.Services.Payment.Application.Dtos;
using Luna.Services.Payment.Application.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Luna.Services.Payment.Api.v1.Controllers;

[ApiController]
[ApiVersion("1.0", Deprecated = false)]
[Route("/api/v{version:ApiVersion}/[controller]")]
public sealed class PaymentsController : LunaControllerBase
{
  private readonly IIdempotentKeyResolver _idempotentKeyResolver;

  private readonly IApiKeyResolver _apiKeyResolver;

  public PaymentsController(
    IMediator mediator,
    IOptions<ApiSettings> apiSettingsOption,
    IIdempotentKeyResolver idempotentKeyResolver,
    IApiKeyResolver apiKeyResolver,
    ILoggerFactory factory)
    : base(apiSettingsOption, mediator, factory.CreateLogger<InfoController>())
  {
    _idempotentKeyResolver = idempotentKeyResolver;
    _apiKeyResolver = apiKeyResolver;
  }

  [HttpGet]
  [SwaggerOperation(OperationId = "GetAsync", Description = "Gets payment by id.",
    Tags = new[] {"Payment"})]
  [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ResponseDto<PaymentDto>))]
  [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(LunaProblemDetails))]
  [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(LunaProblemDetails))]
  [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(LunaProblemDetails))]
  public async Task<IActionResult> GetByIdAsync(
    [Required] [FromQuery] Guid id,
    CancellationToken cancellationToken = default)
  {
    var query = new GetPaymentByIdQuery
    {
      PaymentId = id
    };

    var response = await Mediator.Send(query, cancellationToken);

    return Ok(new ResponseDto<PaymentDto>
    {
      Data = response,
      Status = "Success",
      Version = "1.0"
    });
  }

  [HttpPost]
  [SwaggerOperation(OperationId = "PostAsync", Description = "Creates a single immediate payment.",
    Tags = new[] {"Payment"})]
  [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(ResponseDto<PaymentDto>))]
  [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(LunaProblemDetails))]
  [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(LunaProblemDetails))]
  [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(LunaProblemDetails))]
  public async Task<IActionResult> PostAsync(
    [Required] [FromBody] CreatePaymentDto request,
    CancellationToken cancellationToken = default)
  {
    var command = new CreatePaymentCommand
    {
      IdempotentKey = _idempotentKeyResolver.GetIdempotentKey(),
      ApiKey = _apiKeyResolver.GetApiKey(),
      MerchantId = request.MerchantId,
      CardType = request.Card.CardType,
      Cvv = request.Card.Cvv,
      ExpMonth = request.Card.ExpMonth,
      ExpYear = request.Card.ExpYear,
      NameOnCard = request.Card.NameOnCard,
      Number = request.Card.Number,
      Amount = request.Amount,
      Currency = request.Currency,
      MetaData = request.MetaData.ToDictionary(x => x.Name,
        x => x.Value),
    };

    var response = await Mediator.Send(command, cancellationToken);

    Logger.Log(LogLevel.Information, $"Created a single immediate payment with Id: {response.Id}");

    var location = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/v1/payment/{response.Id}";

    return Created(location, new ResponseDto<PaymentDto>
    {
      Data = response,
      Status = "Success",
      Version = "1.0"
    });
  }
}
