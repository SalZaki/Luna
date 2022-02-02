using System.Diagnostics.CodeAnalysis;

namespace Luna.Framework.AspNetCore;

[ExcludeFromCodeCoverage]
public class ApiSettings
{
  public string? Name { get; set; }

  public string? DatabaseName { get; set; }

  public int DefaultPageNumber { get; set; }

  public int DefaultPageSize { get; set; }

  public string? DocumentationUrl { get; set; }

  public string? Title { get; set; }

  public string? Description { get; set; }

  public string? ContactName { get; set; }

  public string? ContactEmail { get; set; }

  public Uri? TermOfServiceUrl { get; set; }

  public string? LicenseName { get; set; }

  public Uri? LicenseUrl { get; set; }

  public string? Version { get; set; }

  public bool EnableSwagger { get; set; } = false;

  public bool EnableBanner { get; set; } = true;

  public bool IncludeAuthHeader { get; set; } = false;

  public bool IncludeSecurityHeader { get; set; } = false;

  public string? BasePath { get; set; }

  public string? HostPath { get; set; }

  public string ApiKey { get; set; }

  public RequestMasking RequestMasking { get; set; }

  public string GatewayId { get; set; }
}

public class RequestMasking
{
  public bool Enabled { get; set; } = true;
  public string? MaskTemplate { get; set; } = "*********";
}
