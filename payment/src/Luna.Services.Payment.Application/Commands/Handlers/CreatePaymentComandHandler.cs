using FluentValidation;
using Luna.Services.Payment.Application.Data;
using Luna.Services.Payment.Application.Dtos;
using Luna.Services.Payment.Application.Services;
using Luna.Services.Payment.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Metadata = Luna.Services.Payment.Domain.Entities.Metadata;
using Card = Luna.Services.Payment.Domain.Entities.Card;

namespace Luna.Services.Payment.Application.Commands.Handlers;

public sealed class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
  private readonly IMediator _mediator;

  private readonly IRequestMaskingService _requestMaskingService;

  private readonly ILunaPaymentRepository _lunaPaymentRepository;

  private readonly IValidator<CreatePaymentCommand> _validator;

  private readonly ILogger _logger;

  public CreatePaymentCommandHandler(
    IMediator mediator,
    ILunaPaymentRepository lunaPaymentRepository,
    IValidator<CreatePaymentCommand> validator,
    ILoggerFactory loggerFactory)
  {
    _mediator = mediator;
    _lunaPaymentRepository = lunaPaymentRepository;
    _validator = validator;
    _logger = loggerFactory.CreateLogger<CreatePaymentCommandHandler>();
  }

  public async Task<PaymentDto> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
  {
    var validationResult = await _validator.ValidateAsync(command, cancellationToken);

    if (validationResult.IsValid == false)
    {
      _logger.Log(LogLevel.Error, $"Validation failed for {nameof(CreatePaymentCommand)}: {command}");
      throw new ValidationException(validationResult.Errors);
    }

    _logger.Log(LogLevel.Debug, $"Started handling {nameof(CreatePaymentCommand)}: {command}");

    var payment = CreatePaymentEntity(command);
    payment.SubmittedOn = DateTime.UtcNow;

    //TODO Add Automapper
    var bankCommand = new CreateBankChargeCommand
    {
      Amount = command.Amount,
      CardType = command.CardType,
      Currency = command.Currency,
      Cvv = command.Cvv,
      ExpMonth = command.ExpMonth,
      ExpYear = command.ExpYear,
      NameOnCard = command.NameOnCard,
      Number = command.Number
    };

    var bankChargeResponse = await _mediator.Send(bankCommand, cancellationToken);

    //TODO Move this logic somewhere else (not pretty)
    if (bankChargeResponse != null)
    {
      payment.BankResponse = new BankResponse
      {
        BankCode = bankChargeResponse?.BankCode,
        Reason = bankChargeResponse?.Reason,
        Status = bankChargeResponse?.Status
      };
      // Adding 0.05 percent per payment
      if (payment.BankResponse.Status.ToUpper() == "APPROVED")
      {
        payment.EstimatedSettlementCost = (decimal) (0.05 / 100) * payment.Amount;
      }

      payment.Status = "Completed";
      payment.FinalisedOn = DateTime.UtcNow;
    }
    else
    {
      payment.Status = "Completed with error(s)";
      payment.UpdatedOn = DateTime.UtcNow;
    }

    var response = await _lunaPaymentRepository.SaveAsync(payment, cancellationToken);

    _logger.Log(LogLevel.Debug, $"Finished handling {nameof(CreatePaymentCommand)}: {command}");

    //TODO Add Automapper
    return new PaymentDto
    {
      Id = response.Id,
      Amount = response.Amount,
      BankCode = response.BankResponse.BankCode,
      BankReason = response.BankResponse.Reason,
      BankStatus = response.BankResponse.Status,
      Card = new CardDto
      {
        CardType = response.Card.CardType,
        Cvv = response.Card.Cvv,
        ExpMonth = response.Card.ExpMonth,
        ExpYear = response.Card.ExpYear,
        NameOnCard = response.Card.NameOnCard,
        Number = response.Card.Number
      },
      Currency = response.Currency,
      Status = response.Status,
      EstimatedSettlementCost = response.EstimatedSettlementCost,
      FinalisedOn = response.FinalisedOn,
      IdempotentKey = response.IdempotentKey,
      MerchantId = response.MerchantId,
      MetaData = response.Metadata
        .Select(x => new MetadataDto {Name = x.Name, Value = x.Value})
        .ToArray(),
      SubmittedOn = response.SubmittedOn,
      UpdatedOn = response.UpdatedOn
    };
  }

  private static Domain.Entities.Payment CreatePaymentEntity(CreatePaymentCommand command)
  {
    return new Domain.Entities.Payment
    {
      Id = Guid.NewGuid(),
      IdempotentKey = command.IdempotentKey,
      MerchantId = command.MerchantId,
      ApiKey = command.ApiKey,
      Card = new Card
      {
        Id = Guid.NewGuid(),
        CardType = command.CardType,
        Cvv = command.Cvv,
        ExpMonth = command.ExpMonth,
        ExpYear = command.ExpYear,
        NameOnCard = command.NameOnCard,
        Number = command.Number
      },
      Amount = command.Amount,
      Currency = command.Currency,
      Metadata = command.MetaData
        .Select(x => new Metadata {Name = x.Key, Value = x.Value})
        .ToList()
    };
  }
}
