using Luna.Services.Payment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Luna.Services.Payment.Infrastructure.Data.SchemaDefinitions;

public class BankResponseEntitySchemaDefinition : IEntityTypeConfiguration<BankResponse>
{
  public void Configure(EntityTypeBuilder<BankResponse> builder)
  {
    builder.ToTable("BankResponses");

    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id).IsRequired();
    builder.Property(x => x.Reason).HasColumnType("varchar(100)");
    builder.Property(x => x.BankCode).HasColumnType("varchar(100)");
    builder.Property(x => x.Status).HasColumnType("varchar(100)");

    builder.HasOne(c => c.Payment).WithOne(p => p.BankResponse)
      .HasForeignKey<BankResponse>(p => p.PaymentId).OnDelete(DeleteBehavior.Cascade);
  }
}
