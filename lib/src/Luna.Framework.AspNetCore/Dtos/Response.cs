using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Luna.Framework.AspNetCore.Dtos;

[ExcludeFromCodeCoverage]
public class ResponseDto<T> : BaseResponseDto where T : class
{
  [JsonPropertyName("data")]
  public T? Data { get; set; }
}
