using Microsoft.AspNetCore.Mvc;

namespace Luna.Framework.AspNetCore.Responses;

public class LunaProblemDetails : ProblemDetails
{
  public string? DocumentationUrl { get; set; }

  public string? StackTrace { get; set; }
}
