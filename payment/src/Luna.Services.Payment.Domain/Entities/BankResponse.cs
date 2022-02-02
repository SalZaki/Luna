namespace Luna.Services.Payment.Domain.Entities;

public class BankResponse : BaseEntity
{
  public Guid PaymentId { get; set; }

  public Payment Payment { get; private set; }

  public string BankCode { get; set; }

  public string Status { get; set; }

  public string Reason { get; set; }
}
