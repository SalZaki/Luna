using Luna.Services.Payment.Application.Dtos;
using MediatR;

namespace Luna.Services.Payment.Application.Queries;

public sealed class GetPaymentByIdQuery : IRequest<PaymentDto>
{
  public Guid PaymentId { get; set; }
}
