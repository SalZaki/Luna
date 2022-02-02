using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace AcquirerBank.Models;

[ExcludeFromCodeCoverage]
public class BankChargeRequest
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
}
