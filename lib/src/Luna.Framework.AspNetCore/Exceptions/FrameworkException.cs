namespace Luna.Framework.AspNetCore.Exceptions;

public class FrameworkException : Exception
{
  public string Code { get; }

  public FrameworkException()
  {
  }

  public FrameworkException(string code)
  {
    Code = code;
  }

  public FrameworkException(string message, params object[] args)
    : this(string.Empty, message, args)
  {
  }

  public FrameworkException(string code, string message, params object[] args)
    : this(null, code, message, args)
  {
  }

  public FrameworkException(Exception innerException, string message, params object[] args)
    : this(innerException, string.Empty, message, args)
  {
  }

  public FrameworkException(Exception innerException, string code, string message, params object[] args)
    : base(string.Format(message, args), innerException)
  {
    Code = code;
  }
}
