namespace Luna.Framework.AspNetCore.Resolvers;

public interface IIdempotentKeyResolver
{
  Guid GetIdempotentKey();
}
