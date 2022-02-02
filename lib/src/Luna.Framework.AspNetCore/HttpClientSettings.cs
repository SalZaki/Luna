using System.Diagnostics.CodeAnalysis;

namespace Luna.Framework.AspNetCore;

[ExcludeFromCodeCoverage]
public class HttpClientSettings
{
  public string Name { get; set; }
  
  public EndPoint[] Endpoints { get; set; } = Array.Empty<EndPoint>();

  public string BaseAddress { get; set; }

  public int CacheExpirationInMin { get; set; } = 15;

  public string CacheKey { get; set; }

  public int NumberOfExceptionsBeforeCircuitBreaker { get; set; } = 20;

  public int CircuitBreakerFailurePeriodInMin { get; set; } = 1;

  public int[] RetryTimeSpansInSec { get; set; } = { 1, 3, 5 };
}

[ExcludeFromCodeCoverage]
public sealed class EndPoint
{
  public string Name { get; set; }

  public string Url { get; set; }
}
