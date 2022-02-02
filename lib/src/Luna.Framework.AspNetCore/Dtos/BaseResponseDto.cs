using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Luna.Framework.AspNetCore.Dtos;

[ExcludeFromCodeCoverage]
public abstract class BaseResponseDto
{
  [JsonPropertyName("status")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public virtual string? Status { get; set; }

  [JsonPropertyName("message")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public virtual string? Message { get; set; }

  [JsonPropertyName("version")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public virtual string? Version { get; set; }
}
