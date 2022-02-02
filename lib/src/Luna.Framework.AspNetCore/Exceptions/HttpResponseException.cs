using System.Net;
using System.Text.Json;

namespace Luna.Framework.AspNetCore.Exceptions;

public class HttpResponseException : Exception
{
  public HttpStatusCode StatusCode { get; }

  public JsonDocument? Content { get; }

  public Uri Uri { get; }

  public HttpResponseException(HttpStatusCode statusCode, Uri uri, JsonDocument? content)
    : base($"Response status code does not indicate success: {(int)statusCode} ({statusCode}) - {uri}")
  {
    StatusCode = statusCode;
    Content = content;
    Uri = uri;
  }
}
