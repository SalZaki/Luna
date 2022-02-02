using System.Diagnostics.CodeAnalysis;

namespace Luna.Common.Infrastructure.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class ExceptionBase : Exception
{
  protected ExceptionBase(string message)
    : this(message, null)
  {
  }

  protected ExceptionBase(string message, Exception innerEx)
    : base(message, innerEx)
  {
  }
}
