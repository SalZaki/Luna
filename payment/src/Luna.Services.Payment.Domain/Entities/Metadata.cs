namespace Luna.Services.Payment.Domain.Entities;

public class Metadata : BaseEntity
{
  public string Name { get; set; }

  public string Value { get; set; }

  public Guid PaymentId { get; set; }

  public Payment Payment { get; set; }
}
