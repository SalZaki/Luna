using Luna.Services.Payment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Luna.Services.Payment.Infrastructure.Data.SchemaDefinitions;

public class CardEntitySchemaDefinition : IEntityTypeConfiguration<Domain.Entities.Card>
{
  public void Configure(EntityTypeBuilder<Domain.Entities.Card> builder)
  {
    builder.ToTable("Cards");

    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id).IsRequired();
    builder.Property(x => x.CardType).HasColumnType("varchar(10)");
    builder.Property(x => x.ExpMonth).HasColumnType("varchar(2)").IsRequired();
    builder.Property(x => x.ExpYear).HasColumnType("varchar(2)").IsRequired();
    builder.Property(x => x.Cvv).HasMaxLength(3).HasColumnType("varchar(3)").IsRequired();
    builder.Property(x => x.Number).HasColumnType("varchar(100)").IsRequired();
    builder.Property(x => x.NameOnCard).HasColumnType("varchar(100)").IsRequired();

    builder.HasOne(c => c.Payment).WithOne(p => p.Card)
      .HasForeignKey<Card>(p => p.PaymentId).OnDelete(DeleteBehavior.Cascade);
  }
}
