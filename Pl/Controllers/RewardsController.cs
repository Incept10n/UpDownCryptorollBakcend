using AutoMapper;
using Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Filters.FilterAttributes;
using UpDownCryptorollBackend.Models;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("api/rewards")]
[Authorize]
[UsernameAuthorization]
public class RewardsController(
    RewardsService rewardsService,
    IMapper mapper) : ControllerBase
{
    [HttpGet("dailyRewardStatus")]
    public IActionResult GetDailyRewardStatus(string username)
    {
        return Ok(mapper.Map<RewardStatusModel>(rewardsService.GetRewardStatus(username)));
    }

    [HttpPost("collectDailyReward")]
    public IActionResult CollectReward(string username)
    {
        rewardsService.CollectReward(username);
        
        return Ok();
    }
}