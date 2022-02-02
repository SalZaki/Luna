using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Luna.Framework.AspNetCore.Controllers;

[Produces(Constants.MimeTypes.ApplicationJson)]
public abstract class LunaControllerBase : ControllerBase
{
  protected IMediator Mediator { get; }

  protected ApiSettings ApiSettings { get; }

  protected ILogger Logger { get; }

  protected LunaControllerBase(
    IOptions<ApiSettings> apiSettingsOption,
    IMediator mediator,
    ILogger logger) : this(apiSettingsOption, logger)
  {
    Mediator = mediator;
  }

  protected LunaControllerBase(
    IOptions<ApiSettings> apiSettingsOption,
    ILogger logger)
  {
    ApiSettings = apiSettingsOption.Value;
    Logger = logger;
  }
}
