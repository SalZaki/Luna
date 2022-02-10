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

  private readonly IOptions<ApiSettings> _apiSettingsOptions;

  private readonly ILogger _logger;

  private readonly IMediator _mediator;

  protected LunaControllerBase(
    IOptions<ApiSettings> apiSettingsOption,
    IMediator mediator,
    ILogger logger) : this(apiSettingsOption, logger)
  {
    _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    Mediator = _mediator;
  }

  protected LunaControllerBase(
    IOptions<ApiSettings> apiSettingsOption,
    ILogger logger)
  {
    _apiSettingsOptions = apiSettingsOption ?? throw new ArgumentNullException(nameof(apiSettingsOption));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    ApiSettings = _apiSettingsOptions.Value;
    Logger = _logger;
  }
}
