using AutoMapper;
using Bll.Services;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Models;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("/rewards")]
public class RewardsController(
    RewardsService rewardsService,
    IMapper mapper) : ControllerBase
{
    [HttpGet("dailyRewardStatus")]
    public IActionResult GetDailyRewardStatus(string walletAddress)
    {
        return Ok(mapper.Map<RewardStatusModel>(rewardsService.GetRewardStatus(walletAddress)));
    }

    [HttpPost("collectDailyReward")]
    public IActionResult CollectReward(string walletAddress)
    {
        rewardsService.CollectReward(walletAddress);
        
        return Ok();
    }
}