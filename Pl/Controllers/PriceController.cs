using Bll.Services;
using Dal.Enums;
using Microsoft.AspNetCore.Mvc;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("/livePrice")]
public class PriceController(GameLogicService gameLogicService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPrice(string sym)
    {
        if (!Enum.TryParse(sym, true, out Coin inputCoin))
        {
            return BadRequest("Uknown coin type");
        }
        
        return Ok(await gameLogicService.GetCurrentPrice(inputCoin));
    }
}