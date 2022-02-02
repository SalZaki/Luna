using Luna.Services.Payment.Application.Dtos;

namespace Luna.Services.Payment.Application.Services;

public interface IAcquirerBankApiClient
{
  Task<BankResponseDto> PostChargeAsync(BankRequestDto request, CancellationToken cancellationToken);
}
