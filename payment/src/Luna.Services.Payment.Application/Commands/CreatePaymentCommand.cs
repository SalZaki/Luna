using Luna.Services.Payment.Application.Dtos;
using MediatR;

namespace Luna.Services.Payment.Application.Commands;

public class CreatePaymentCommand : IRequest<PaymentDto>
{
  public Guid IdempotentKey { get; set; }

  public Guid ApiKey { get; set; }

  public Guid MerchantId { get; set; }

  public Dictionary<string, string> MetaData { get; set; } = new();

  public decimal Amount { get; set; }

  public string Currency { get; set; }

  public string CardType { get; set; }

  public string ExpMonth { get; set; }

  public string ExpYear { get; set; }

  public string Cvv { get; set; }

  public string Number { get; set; }

  public string NameOnCard { get; set; }
}
