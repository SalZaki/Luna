using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Luna.Framework.AspNetCore.Dtos;

[ExcludeFromCodeCoverage]
public sealed class ApiInformationDto
{
  [JsonPropertyName("api_name")]
  public string? ApiName { get; set; }

  [JsonPropertyName("api_version")]
  public string? ApiVersion { get; set; }

  [JsonPropertyName("environment")]
  public string? Environment { get; set; }
}
