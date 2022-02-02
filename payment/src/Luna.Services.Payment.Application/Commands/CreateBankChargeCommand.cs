using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Luna.Services.Payment.Application.Dtos;
using MediatR;

namespace Luna.Services.Payment.Application.Commands;

[ExcludeFromCodeCoverage]
public class CreateBankChargeCommand : IRequest<BankResponseDto>
{
  [JsonPropertyName("card_type")]
  public string CardType { get; set; }

  [JsonPropertyName("exp_month")]
  public string ExpMonth { get; set; }

  [JsonPropertyName("exp_year")]
  public string ExpYear { get; set; }

  [JsonPropertyName("cvv")]
  public string Cvv { get; set; }

  [JsonPropertyName("number")]
  public string Number { get; set; }

  [JsonPropertyName("name_on_card")]
  public string NameOnCard { get; set; }

  [JsonPropertyName("amount")]
  public decimal Amount { get; set; }

  [JsonPropertyName("currency")]
  public string Currency { get; set; }

  [JsonPropertyName("gateway_id")]
  public Guid GatewayId { get; set; }
}
