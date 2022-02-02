// using System.Net.Http.Headers;
// using System.Text;
// using System.Text.Json;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using Polly;
// using JsonSerializer = System.Text.Json.JsonSerializer;
//
// namespace Luna.Framework.AspNetCore.Http;
//
// public class LunaHttpClient : IHttpClient
// {
//   private readonly HttpClient _httpClient;
//   private readonly HttpClientSettings _httpClientSettings;
//   private readonly ILogger<LunaHttpClient> _logger;
//
//   private static readonly StringContent EmptyJson =
//     new StringContent("{}", Encoding.UTF8, Constants.MimeTypes.ApplicationJson);
//
//   private readonly JsonSerializerOptions _jsonSerializerOptions;
//
//   public LunaHttpClient(HttpClient httpClient, IOptions<HttpClientSettings> options, ILoggerFactory loggerFactory)
//   {
//     _httpClient = httpClient;
//     _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
//     _httpClientSettings = options.Value;
//     _jsonSerializerOptions = new JsonSerializerOptions();
//     _logger = loggerFactory.CreateLogger<LunaHttpClient>();
//   }
//
//   public virtual Task<HttpResponseMessage> GetAsync(string uri)
//     => SendAsync(uri, Method.Get);
//
//   public virtual Task<T> GetAsync<T>(string uri)
//     => SendAsync<T>(uri, Method.Get);
//
//   public virtual Task<HttpResponseMessage> PostAsync(string uri, object data = null)
//     => SendAsync(uri, Method.Post, data);
//
//   public virtual Task<T> PostAsync<T>(string uri, object data = null)
//     => SendAsync<T>(uri, Method.Post, data);
//
//   public virtual Task<HttpResponseMessage> PutAsync(string uri, object data = null)
//     => SendAsync(uri, Method.Put, data);
//
//   public virtual Task<T> PutAsync<T>(string uri, object data = null)
//     => SendAsync<T>(uri, Method.Put, data);
//
//   public virtual Task<HttpResponseMessage> DeleteAsync(string uri)
//     => SendAsync(uri, Method.Delete);
//
//   protected virtual async Task<T> SendAsync<T>(string uri, Method method, object data = null)
//   {
//     var response = await SendAsync(uri, method, data);
//     if (!response.IsSuccessStatusCode)
//     {
//       return default;
//     }
//
//     var stream = await response.Content.ReadAsStreamAsync();
//
//     return await DeserializeJsonFromStream<T>(stream, _jsonSerializerOptions);
//   }
//
//   protected virtual Task<HttpResponseMessage> SendAsync(string uri, Method method, object data = null)
//     => Policy.Handle<Exception>()
//       .WaitAndRetryAsync(_httpClientSettings.Retries, r => TimeSpan.FromSeconds(Math.Pow(2, r)))
//       .ExecuteAsync(async () =>
//       {
//         var methodName = method.ToString().ToUpperInvariant();
//         var requestUri = uri.StartsWith("https://") ? uri : $"http://{uri}";
//         _logger.LogDebug($"Sending HTTP {methodName} request to URI: {uri}");
//         var response = await GetResponseAsync(requestUri, method, "data");
//         if (response.IsSuccessStatusCode)
//         {
//           _logger.LogDebug($"Received a valid response to HTTP {methodName} request from URI: " +
//                            $"{requestUri}{Environment.NewLine}{response}");
//         }
//         else
//         {
//           _logger.LogError($"Received an invalid response to HTTP {methodName} request from URI: " +
//                            $"{requestUri}{Environment.NewLine}{response}");
//         }
//
//         return response;
//       });
//
//   protected virtual async Task<HttpResponseMessage> GetResponseAsync(string uri, Method method, object data = null)
//   {
//     switch (method)
//     {
//       case Method.Get:
//         return _httpClient.GetAsync(uri).Result;
//       case Method.Post:
//         return await _httpClient.PostAsync(uri, GetJsonPayload(data));
//       case Method.Put:
//         return await _httpClient.PutAsync(uri, GetJsonPayload(data));
//       case Method.Delete:
//         return await _httpClient.DeleteAsync(uri);
//       default:
//         throw new InvalidOperationException($"Unsupported HTTP method: " +
//                                             $"{method.ToString().ToUpperInvariant()}.");
//     }
//   }
//
//   private static StringContent GetJsonPayload(object data)
//   {
//     if (data is null)
//     {
//       return EmptyJson;
//     }
//     else
//     {
//       var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8,
//         Constants.MimeTypes.ApplicationJson);
//       content.Headers.ContentType = new MediaTypeHeaderValue(Constants.MimeTypes.ApplicationJson);
//
//       return content;
//     }
//   }
//
//   private static async Task<T> DeserializeJsonFromStream<T>(Stream stream, JsonSerializerOptions options)
//   {
//     if (stream == null || stream.CanRead == false)
//     {
//       return default;
//     }
//     return await JsonSerializer.DeserializeAsync<T>(stream, options);
//   }
//
//   protected enum Method
//   {
//     Get,
//     Post,
//     Put,
//     Delete
//   }
// }
