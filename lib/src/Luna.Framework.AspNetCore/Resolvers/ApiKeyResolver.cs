using Microsoft.AspNetCore.Http;

namespace Luna.Framework.AspNetCore.Resolvers;

public sealed class ApiKeyResolver : IApiKeyResolver
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public ApiKeyResolver(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public Guid GetApiKey()
  {
    var key = _httpContextAccessor?.HttpContext?.Request?.Headers[Constants.RequestHeaderKeys.ApiKey];

    return Guid.TryParse(key, out var apiKey) ? apiKey : default;
  }
}
