using Luna.Services.Payment.Application.Data;
using Luna.Services.Payment.Infrastructure.Data.SchemaDefinitions;
using Microsoft.EntityFrameworkCore;

namespace Luna.Services.Payment.Infrastructure.Data;

public class LunaPaymentDbContext : DbContext
{
  public DbSet<Domain.Entities.Card> Cards { get; set; }

  public DbSet<Domain.Entities.Payment> Payments { get; set; }

  public LunaPaymentDbContext(DbContextOptions<LunaPaymentDbContext> options)
    : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new PaymentEntitySchemaDefinition());
    modelBuilder.ApplyConfiguration(new CardEntitySchemaDefinition());
    modelBuilder.ApplyConfiguration(new MetadataEntitySchemaDefinition());
    modelBuilder.ApplyConfiguration(new BankResponseEntitySchemaDefinition());
    base.OnModelCreating(modelBuilder);
  }
}
