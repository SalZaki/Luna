namespace Luna.Services.Payment.Domain.Entities;

public class Card : BaseEntity
{
  public Guid PaymentId { get; set; }

  public Payment Payment { get; private set; }

  public string CardType { get; set; }

  public string ExpMonth { get; set; }

  public string ExpYear { get; set; }

  public string Cvv { get; set; }

  public string Number { get; set; }

  public string NameOnCard { get; set; }
}
