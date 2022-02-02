using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Luna.Services.Payment.Infrastructure.Data.SchemaDefinitions;

public class MetadataEntitySchemaDefinition : IEntityTypeConfiguration<Domain.Entities.Metadata>
{
  public void Configure(EntityTypeBuilder<Domain.Entities.Metadata> builder)
  {
    builder.ToTable("Metadata");

    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id).IsRequired();
    builder.Property(x => x.Name).HasColumnType("varchar(100)");
    builder.Property(x => x.Value).HasColumnType("varchar(100)");

    builder.HasOne(c => c.Payment).WithMany(p => p.Metadata)
      .HasForeignKey(p => p.PaymentId).OnDelete(DeleteBehavior.Cascade);
  }
}
