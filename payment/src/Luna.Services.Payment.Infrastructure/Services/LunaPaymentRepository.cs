using Luna.Services.Payment.Application.Data;
using Luna.Services.Payment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Luna.Services.Payment.Infrastructure.Services;

public class LunaPaymentRepository : ILunaPaymentRepository
{
  private readonly LunaPaymentDbContext _dbContext;

  public LunaPaymentRepository(
    IDbContextFactory<LunaPaymentDbContext> dbContext)
  {
    _dbContext = dbContext.CreateDbContext();
  }

  public async Task<Domain.Entities.Payment> SaveAsync(Domain.Entities.Payment payment,
    CancellationToken cancellationToken)
  {
    var result = await _dbContext.Payments.AddAsync(payment, cancellationToken);
    await _dbContext.SaveChangesAsync(cancellationToken);
    return result.Entity;
  }

  public async Task<Domain.Entities.Payment> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return await _dbContext.Payments.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
  }

  public async Task<Domain.Entities.Payment> GetByIdempotentKeyAsync(Guid key, CancellationToken cancellationToken)
  {
    return await _dbContext.Payments
      .Include(c => c.Card)
      .Include(b=>b.BankResponse)
      .SingleOrDefaultAsync(x => x.IdempotentKey == key, cancellationToken);
  }
}
