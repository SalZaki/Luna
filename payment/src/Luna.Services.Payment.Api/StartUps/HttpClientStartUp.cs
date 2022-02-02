using System.Net.Http.Headers;
using Luna.Framework.AspNetCore;
using Luna.Services.Payment.Application.Services;
using Luna.Services.Payment.Infrastructure.Clients;
using Microsoft.Extensions.Options;
using Polly;

namespace Luna.Services.Payment.Api.StartUps;

public static class HttpClientStartUp
{
  public static IServiceCollection AddAcquirerBankHttpClient(
    this IServiceCollection services)
  {
    var httpClientSettings  = services.BuildServiceProvider().GetRequiredService<IOptions<HttpClientSettings>>().Value;

    services.AddHttpClient<IAcquirerBankApiClient, AcquirerBankApiClient>(

        httpClient =>
        {
          var baseAddress =   httpClientSettings.BaseAddress;
          if (!baseAddress.EndsWith("/", StringComparison.CurrentCulture))
          {
            baseAddress = $"{baseAddress}/";
          }

          httpClient.BaseAddress = new Uri(baseAddress, UriKind.Absolute);
          httpClient.DefaultRequestHeaders?.Accept.Add(
            new MediaTypeWithQualityHeaderValue(Constants.MimeTypes.ApplicationJson));
          httpClient.DefaultRequestHeaders.Host = httpClientSettings.Name;
        })

      .AddTransientHttpErrorPolicy(c =>
        c.WaitAndRetryAsync(httpClientSettings.RetryTimeSpansInSec.Select(x => TimeSpan.FromSeconds(x))
          .Distinct()))

      .AddTransientHttpErrorPolicy(c =>
        c.CircuitBreakerAsync(httpClientSettings.NumberOfExceptionsBeforeCircuitBreaker,
          TimeSpan.FromMinutes(httpClientSettings.CircuitBreakerFailurePeriodInMin)));

    return services;
  }
}
