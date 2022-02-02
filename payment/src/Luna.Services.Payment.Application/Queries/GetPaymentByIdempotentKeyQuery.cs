using Luna.Services.Payment.Application.Dtos;
using MediatR;

namespace Luna.Services.Payment.Application.Queries;

public sealed class GetPaymentByIdempotentKeyQuery : IRequest<PaymentDto>
{
  public Guid IdempotentKey { get; set; }
}
