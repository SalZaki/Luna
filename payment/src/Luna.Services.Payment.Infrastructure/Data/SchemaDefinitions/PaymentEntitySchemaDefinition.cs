using Luna.Services.Payment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Luna.Services.Payment.Infrastructure.Data.SchemaDefinitions;

public class PaymentEntitySchemaDefinition : IEntityTypeConfiguration<Domain.Entities.Payment>
{
  public void Configure(EntityTypeBuilder<Domain.Entities.Payment> builder)
  {
    builder.ToTable("Payments");

    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id).IsRequired();
    builder.Property(x => x.MerchantId);
    builder.Property(x => x.IdempotentKey);
    builder.Property(x => x.ApiKey);
    builder.Property(x => x.EstimatedSettlementCost).HasColumnType("decimal(10,2)");
    builder.Property(x => x.Amount).HasColumnType("decimal(10,2)").IsRequired();
    builder.Property(x => x.SubmittedOn).HasColumnType("datetime");
    builder.Property(x => x.UpdatedOn).HasColumnType("datetime");
    builder.Property(x => x.FinalisedOn).HasColumnType("datetime");
  }
}
