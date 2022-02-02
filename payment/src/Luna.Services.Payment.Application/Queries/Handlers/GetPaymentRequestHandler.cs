using FluentValidation;
using Luna.Framework.AspNetCore;
using Luna.Services.Payment.Application.Data;
using Luna.Services.Payment.Application.Dtos;
using Luna.Services.Payment.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Luna.Services.Payment.Application.Queries.Handlers;

public class GetPaymentQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto>,
  IRequestHandler<GetPaymentByIdempotentKeyQuery, PaymentDto>
{
  private readonly IValidator<GetPaymentByIdQuery> _paymentByIdQueryValidator;

  private readonly IValidator<GetPaymentByIdempotentKeyQuery> _paymentByIdempotentKeyQueryValidator;

  private readonly ApiSettings _apiSettings;

  private readonly IRequestMaskingService _requestMaskingService;

  private readonly ILunaPaymentRepository _lunaPaymentRepository;

  private readonly ILogger _logger;

  public GetPaymentQueryHandler(
    IValidator<GetPaymentByIdQuery> paymentByIdQueryValidator,
    IValidator<GetPaymentByIdempotentKeyQuery> paymentByIdempotentKeyQueryValidator,
    IOptions<ApiSettings> apiSettingsOptions,
    IRequestMaskingService requestMaskingService,
    ILunaPaymentRepository lunaPaymentRepository,
    ILoggerFactory loggerFactory)
  {
    _paymentByIdQueryValidator = paymentByIdQueryValidator;
    _paymentByIdempotentKeyQueryValidator = paymentByIdempotentKeyQueryValidator;
    _apiSettings = apiSettingsOptions.Value;
    _requestMaskingService = requestMaskingService;
    _lunaPaymentRepository = lunaPaymentRepository;
    _logger = loggerFactory.CreateLogger<GetPaymentQueryHandler>();
  }

  public async Task<PaymentDto> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
  {
    var validationResult = await _paymentByIdQueryValidator.ValidateAsync(query, cancellationToken);

    if (validationResult.IsValid == false)
    {
      _logger.Log(LogLevel.Error, $"Validation failed for {nameof(GetPaymentByIdQuery)}: {query}");
      throw new ValidationException(validationResult.Errors);
    }

    _logger.Log(LogLevel.Debug, $"Started handling {nameof(GetPaymentByIdQuery)}: {query}");

    var payment = await _lunaPaymentRepository.GetByIdAsync(query.PaymentId, cancellationToken: cancellationToken);

    _logger.Log(LogLevel.Debug, $"Finished handling {nameof(GetPaymentByIdQuery)}: {query}");

    return payment == null ? null : CreatePaymentDto(payment);
  }

  public async Task<PaymentDto> Handle(GetPaymentByIdempotentKeyQuery query, CancellationToken cancellationToken)
  {
    var validationResult = await _paymentByIdempotentKeyQueryValidator.ValidateAsync(query, cancellationToken);

    if (validationResult.IsValid == false)
    {
      _logger.Log(LogLevel.Error, $"Validation failed for {nameof(GetPaymentByIdempotentKeyQuery)}: {query}");
      throw new ValidationException(validationResult.Errors);
    }

    _logger.Log(LogLevel.Debug, $"Started handling {nameof(GetPaymentByIdempotentKeyQuery)}: {query}");

    var payment =
      await _lunaPaymentRepository.GetByIdempotentKeyAsync(query.IdempotentKey, cancellationToken: cancellationToken);

    _logger.Log(LogLevel.Debug, $"Finished handling {nameof(GetPaymentByIdempotentKeyQuery)}: {query}");

    return payment == null ? null : CreatePaymentDto(payment);
  }

  private PaymentDto CreatePaymentDto(Domain.Entities.Payment payment)
  {
    //TODO Add AutoMapper
    var card = new CardDto
    {
      Number = _apiSettings.RequestMasking.Enabled
        ? _requestMaskingService.Mask(char.Parse(_apiSettings.RequestMasking.MaskTemplate), payment.Card.Number.Length)
        : payment.Card.Number,
      CardType = payment.Card.CardType,
      Cvv = payment.Card.Cvv,
      ExpMonth = payment.Card.ExpMonth,
      ExpYear = payment.Card.ExpYear,
      NameOnCard = payment.Card.NameOnCard
    };

    return new PaymentDto
    {
      Id = payment.Id,
      Amount = payment.Amount,
      BankCode = payment.BankResponse.BankCode,
      BankReason = payment.BankResponse.Reason,
      BankStatus = payment.BankResponse.Status,
      Card = card,
      Currency = payment.Currency,
      Status = payment.Status,
      EstimatedSettlementCost = payment.EstimatedSettlementCost,
      FinalisedOn = payment.FinalisedOn,
      IdempotentKey = payment.IdempotentKey,
      MerchantId = payment.MerchantId,
      MetaData = payment.Metadata
        .Select(x => new MetadataDto {Name = x.Name, Value = x.Value})
        .ToArray(),
      SubmittedOn = payment.SubmittedOn,
      UpdatedOn = payment.UpdatedOn
    };
  }
}
