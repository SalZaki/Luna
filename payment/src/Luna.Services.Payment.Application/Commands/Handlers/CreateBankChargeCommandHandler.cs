using Luna.Framework.AspNetCore;
using Luna.Services.Payment.Application.Dtos;
using Luna.Services.Payment.Application.Services;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Luna.Services.Payment.Application.Commands.Handlers;

public class CreateBankChargeCommandHandler : IRequestHandler<CreateBankChargeCommand, BankResponseDto>
{
  private readonly ApiSettings _apiSettings;

  private readonly IAcquirerBankApiClient _acquirerBankApiClient;

  private readonly ILogger _logger;

  public CreateBankChargeCommandHandler(
    IOptions<ApiSettings> apiSettingsOptions,
    IAcquirerBankApiClient acquirerBankApiClient,
    ILoggerFactory loggerFactory)
  {
    _apiSettings = apiSettingsOptions.Value;
    _acquirerBankApiClient = acquirerBankApiClient;
    _logger = loggerFactory.CreateLogger<CreateBankChargeCommandHandler>();
  }

  public async Task<BankResponseDto> Handle(CreateBankChargeCommand command, CancellationToken cancellationToken)
  {
    //TODO Add command validator
    _logger.Log(LogLevel.Debug, $"Started handling {nameof(CreateBankChargeCommand)}: {command}");

    //TODO Add AutoMapper
    var bankChargeRequest = new BankRequestDto
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

    var response =  await _acquirerBankApiClient.PostChargeAsync(bankChargeRequest, cancellationToken);

    _logger.Log(LogLevel.Debug, $"Finished handling {nameof(CreateBankChargeCommand)}: {command}");

    return response;
  }
}
