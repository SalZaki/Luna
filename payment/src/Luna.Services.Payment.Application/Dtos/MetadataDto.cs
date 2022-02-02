using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Luna.Services.Payment.Application.Dtos;

[ExcludeFromCodeCoverage]
public sealed class MetadataDto
{
  [JsonPropertyName("name")]
  public string Name { get; set; }

  [JsonPropertyName("value")]
  public string Value { get; set; }
}
