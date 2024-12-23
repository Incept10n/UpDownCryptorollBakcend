using Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Filters.FilterAttributes;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("api/referralLinks")]
[Authorize]
public class ReferralLinkController(
    ReferralService referralService) : ControllerBase
{
    
    [HttpGet]
    [UsernameAuthorization]
    public async Task<IActionResult> GetUserReferralLinksAsync(string username)
    {
        return Ok(await referralService.GetUserReferralSaltAsync(username));
    }

    [HttpPost("visit")]
    [UsernameAuthorization("visitorName")]
    public async Task<IActionResult> VisitReferralLinkAsync(string visitorName, string referralSalt)
    {
        await referralService.VisitReferralAsync(visitorName, referralSalt);

        return Ok();
    }
}