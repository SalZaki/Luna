using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Luna.Services.Payment.Application.Dtos;

[ExcludeFromCodeCoverage]
public class BankResponseDto
{
  [JsonPropertyName("transaction_id")]
  public string TransactionId { get; set; }

  [JsonPropertyName("bank_code")]
  public string BankCode { get; set; }

  [JsonPropertyName("status")]
  public string Status { get; set; }

  [JsonPropertyName("reason")]
  public string Reason { get; set; }
}
