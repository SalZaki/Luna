namespace Luna.Services.Payment.Application.Services;

public interface IRequestMaskingService
{
  string Mask(char maskingTemplate, int length);
}
