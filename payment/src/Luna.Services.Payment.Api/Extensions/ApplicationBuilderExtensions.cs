using Luna.Services.Payment.Api.Middlewares;

namespace Luna.Services.Payment.Api.Extensions;

public static class Application
{
  public static IApplicationBuilder UseIdempotentMiddleware(this IApplicationBuilder app)
  {
    if (app == null)
    {
      throw new ArgumentNullException(nameof(app));
    }

    return app.UseMiddleware<IdempotentMiddleware>();
  }
}
