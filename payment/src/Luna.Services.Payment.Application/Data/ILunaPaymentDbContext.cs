using Microsoft.EntityFrameworkCore;

namespace Luna.Services.Payment.Application.Data;
//
// public interface ILunaPaymentDbContext
// {
//   DbSet<Domain.Entities.Card> Cards { get; set; }
//
//   DbSet<Domain.Entities.Payment> Payments { get; set; }
//
//   Task<int> SaveChangesAsync(CancellationToken cancellationToken);
// }

public interface ILunaPaymentRepository
{
  Task<Domain.Entities.Payment> SaveAsync(Domain.Entities.Payment payment, CancellationToken cancellationToken);

  Task<Domain.Entities.Payment> GetByIdAsync(Guid id, CancellationToken cancellationToken);

  Task<Domain.Entities.Payment> GetByIdempotentKeyAsync(Guid key, CancellationToken cancellationToken);
}
