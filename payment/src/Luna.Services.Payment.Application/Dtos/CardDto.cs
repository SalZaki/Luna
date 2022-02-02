using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Luna.Services.Payment.Application.Dtos;

[ExcludeFromCodeCoverage]
public sealed class CardDto
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
}
