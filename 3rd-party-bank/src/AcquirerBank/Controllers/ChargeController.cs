using AcquirerBank.Models;
using Microsoft.AspNetCore.Mvc;

namespace AcquirerBank.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ChargeController : ControllerBase
{
  private readonly ILogger<ChargeController> _logger;

  public ChargeController(ILogger<ChargeController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
  public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
  {
    return Ok("AcquirerBank Api is running successfully.");
  }

  [HttpPost]
  public async Task<IActionResult> PostAsync([FromBody] BankChargeRequest request,
    CancellationToken cancellationToken = default)
  {
    var response = new BankChargeResponse();

    if (request.CardType.ToUpper() == "VISA" &&
        request.Currency.ToUpper() == "GBP" &&
        request.Amount > 0)
    {
      response.BankCode = "123";
      response.Reason = "Success";
      response.Status = "Approved";
      return Created("/", response);
    }

    response.BankCode = "456";
    response.Reason = "Failed";
    response.Status = "Rejected";
    return BadRequest(response);
  }
}
