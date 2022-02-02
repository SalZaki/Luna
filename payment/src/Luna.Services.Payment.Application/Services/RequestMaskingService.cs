namespace Luna.Services.Payment.Application.Services;

public sealed class RequestMaskingService : IRequestMaskingService
{
  public string Mask(char maskingTemplate, int length)
  {
    return new string(maskingTemplate, length);
  }
}
