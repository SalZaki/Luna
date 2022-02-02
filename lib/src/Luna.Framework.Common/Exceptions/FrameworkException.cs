using System.Diagnostics.CodeAnalysis;

namespace Luna.Common.Infrastructure.Exceptions;

[ExcludeFromCodeCoverage]
public class FrameworkException : ExceptionBase
{
  public FrameworkException(string message)
    : base(message, null) { }

  public FrameworkException(string message, Exception ex)
    : base(message, ex) { }
}
