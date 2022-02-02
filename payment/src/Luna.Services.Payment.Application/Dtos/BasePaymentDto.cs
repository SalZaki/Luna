using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Luna.Services.Payment.Application.Dtos;

[ExcludeFromCodeCoverage]
public abstract class BasePaymentDto
{
  [JsonPropertyName("merchant_id")]
  public Guid MerchantId { get; set; }

  [JsonPropertyName("meta_data")]
  public MetadataDto[] MetaData { get; set; }

  [JsonPropertyName("card")]
  public CardDto Card { get; set; }

  [JsonPropertyName("amount")]
  public decimal Amount { get; set; }

  [JsonPropertyName("currency")]
  public string Currency { get; set; }
}
