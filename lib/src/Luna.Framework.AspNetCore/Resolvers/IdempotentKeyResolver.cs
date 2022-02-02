using Microsoft.AspNetCore.Http;

namespace Luna.Framework.AspNetCore.Resolvers;

public sealed class IdempotentKeyResolver : IIdempotentKeyResolver
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public IdempotentKeyResolver(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public Guid GetIdempotentKey()
  {
    var key = _httpContextAccessor?.HttpContext?.Request?.Headers[Constants.RequestHeaderKeys.IdempotentKey];

    return Guid.TryParse(key, out var idempotentKey) ? idempotentKey : default;
  }
}
