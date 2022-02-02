using Luna.Framework.AspNetCore;
using Luna.Framework.AspNetCore.Controllers;
using Luna.Framework.AspNetCore.Dtos;
using Luna.Framework.AspNetCore.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Luna.Services.Payment.Api.v1.Controllers;

[ApiController]
[ApiVersion("1.0", Deprecated = false)]
[Route("/api/v{version:ApiVersion}/[controller]")]
public class InfoController : LunaControllerBase
{
  private readonly IWebHostEnvironment _webHostEnvironment;

  public InfoController(
    IOptions<ApiSettings> apiSettingsOption,
    IWebHostEnvironment webHostEnvironment,
    ILoggerFactory factory)
    : base(apiSettingsOption, factory.CreateLogger<InfoController>())
  {
    _webHostEnvironment = webHostEnvironment;
  }

  [HttpGet]
  [SwaggerOperation(OperationId = "GetInformation", Description = "Gets information about Payment Api",
    Tags = new[] {"Luna Payment Api"})]
  [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ResponseDto<ApiInformationDto>))]
  [SwaggerResponse(StatusCodes.Status401Unauthorized, "A server error has occured, please try later.",
    typeof(LunaProblemDetails))]
  [SwaggerResponse(StatusCodes.Status500InternalServerError, "A server error has occured, please try later.",
    typeof(LunaProblemDetails))]
  public ActionResult GetInformation()
  {
    Logger.Log(LogLevel.Information, $"Get api information request has been received.");

    var response = new ResponseDto<ApiInformationDto>
    {
      Data = new ApiInformationDto
      {
        ApiName = ApiSettings.Name,
        ApiVersion = ApiSettings.Version,
        Environment = _webHostEnvironment.EnvironmentName
      },
      Status = "Success",
      Version = ApiSettings.Version,
    };

    return Ok(response);
  }
}
