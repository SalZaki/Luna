using System.Text.Json.Serialization;

namespace AcquirerBank.Models;

public class BankChargeResponse
{
  [JsonPropertyName("bank_code")]
  public string BankCode { get; set; }

  [JsonPropertyName("status")]
  public string Status { get; set; }

  [JsonPropertyName("reason")]
  public string Reason { get; set; }
}
