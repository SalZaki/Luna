using System.Text;
using System.Text.Json;
using Luna.Framework.AspNetCore;
using Luna.Framework.AspNetCore.Exceptions;
using Luna.Services.Payment.Application.Dtos;
using Luna.Services.Payment.Application.Services;
using Luna.Services.Payment.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Luna.Services.Payment.Infrastructure.Clients;

public sealed class AcquirerBankApiClient : IAcquirerBankApiClient
{
  private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
  {
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  private readonly HttpClient _client;

  private readonly HttpClientSettings _httpClientSettings;

  public AcquirerBankApiClient(
    HttpClient client,
    IOptions<HttpClientSettings> httpClientSettingsOptions)
  {
    _client = client;
    _httpClientSettings = httpClientSettingsOptions.Value;
  }

  public async Task<BankResponseDto> PostChargeAsync(BankRequestDto request, CancellationToken cancellationToken)
  {
    var chargeEndPoint = _httpClientSettings.Endpoints.Single(x => x.Name == "Charge");

    var content = CreateContent(request);
    var contentStream =
      await GetResponseContentStreamAsync(chargeEndPoint.Url, HttpMethod.Post, content, cancellationToken);
    return await JsonSerializer.DeserializeAsync<BankResponseDto>(contentStream, Options,
      cancellationToken);
  }

  private static StringContent? CreateContent(BankRequestDto value)
  {
    return new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, Constants.MimeTypes.ApplicationJson);
  }

  private async Task<Stream> GetResponseContentStreamAsync(
    string requestUri,
    HttpMethod verb,
    HttpContent? content = default,
    CancellationToken cancellationToken = default)
  {
    var request = new HttpRequestMessage(verb, requestUri);
    request.Content = content;
    var response = await _client.SendAsync(request, cancellationToken);

    await CheckResponse(response);

    return await response.Content.ReadAsStreamAsync(cancellationToken);
  }

  private async Task CheckResponse(HttpResponseMessage response)
  {
    if (response.IsSuccessStatusCode) return;

    // 4xx - client errors, special handling as we want the error message to survive
    if ((int) response.StatusCode >= 400 && (int) response.StatusCode <= 499)
    {
      var responseContent = await response.Content.ReadAsStringAsync();

      throw new HttpResponseException(
        response.StatusCode,
        _client.BaseAddress,
        JsonDocument.Parse(responseContent));
    }

    response.EnsureSuccessStatusCode();
  }
}
