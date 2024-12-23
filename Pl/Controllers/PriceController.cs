using Bll.Services;
using Dal.Enums;
using Microsoft.AspNetCore.Mvc;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("api/livePrice")]
public class PriceController(CurrentPriceService currentPriceService) : ControllerBase
{
    [HttpGet]
    public IActionResult  GetPrice(string sym)
    {
        if (!Enum.TryParse(sym, true, out Coin inputCoin))
        {
            return BadRequest("Uknown coin type");
        }
        
        return Ok(currentPriceService.GetCurrentPrice(inputCoin));
    }
}