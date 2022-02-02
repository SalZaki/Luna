namespace Luna.Framework.AspNetCore;

public static class Constants
{
  public static class MimeTypes
  {
    public const string ApplicationJson = "application/json";

    public const string ApplicationProblemJson = "application/problem+json";
  }

  public static class RequestHeaderKeys
  {
    public const string IdempotentKey = "idempotent-key";

    public const string ApiKey = "api-Key";
  }

  public static class Errors
  {
    public static class Server
    {
      public const string ErrorTitle = "Server Error";

      public const string ErrorCode = "10001";

      public const string ErrorMessage =
        "Luna Payment API is unavailable and returned an internal error code. Please contact system administrator or try again later";
    }

    public static class Validation
    {
      public const string ErrorTitle = "Validation Error";

      public const string ErrorCode = "20001";

      public const string ErrorMessage = "Luan Payment Api validation has failed, due to following error(s)";
    }

    public static class UnauthorizedAccess
    {
      public const string ErrorTitle = "Access Denied";

      public const string ErrorCode = "30001";

      public const string ErrorMessage = "You do not have permission to perform this action or access this resource.";
    }
  }

  public static class Configuration
  {
    public const string ApiSettingsSection = "ApiSettings";

    public const string HttpClientSettingsSection = "HttpClientSettings";
  }
}
