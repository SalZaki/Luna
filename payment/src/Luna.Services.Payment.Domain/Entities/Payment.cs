namespace Luna.Services.Payment.Domain.Entities;

public sealed class Payment : BaseEntity
{
  public Guid IdempotentKey { get; set; }

  public Guid ApiKey { get; set; }

  public Guid MerchantId { get; set; }

  public string Status { get; set; }

  public Card Card { get; set; }

  public BankResponse BankResponse { get; set; }

  public decimal Amount { get; set; }

  public string Currency { get; set; }

  public decimal EstimatedSettlementCost { get; set; }

  public List<Metadata> Metadata { get; set; } = new();

  public DateTime SubmittedOn { get; set; }

  public DateTime UpdatedOn { get; set; }

  public DateTime FinalisedOn { get; set; }
}
