using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Luna.Services.Payment.Application.Dtos;

[ExcludeFromCodeCoverage]
public sealed class PaymentDto : BasePaymentDto
{
  [JsonPropertyName("payment_id")]
  public Guid Id { get; set; }

  [JsonPropertyName("status")]
  public string Status { get; set; }

  [JsonPropertyName("finalised_on")]
  public DateTime FinalisedOn { get; set; }

  [JsonPropertyName("updated_on")]
  public DateTime UpdatedOn { get; set; }

  [JsonPropertyName("submitted_on")]
  public DateTime SubmittedOn { get; set; }

  [JsonPropertyName("estimated_settlement_cost")]
  public decimal EstimatedSettlementCost { get; set; }

  [JsonPropertyName("bank_code")]
  public string BankCode { get; set; }

  [JsonPropertyName("bank_status")]
  public string BankStatus { get; set; }

  [JsonPropertyName("bank_reason")]
  public string BankReason { get; set; }

  [JsonPropertyName("idempotent_key")]
  public Guid IdempotentKey { get; set; }
}
