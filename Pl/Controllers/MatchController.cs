using AutoMapper;
using Bll.Dtos;
using Bll.Services;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Models;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("/match")]
public class MatchController(
    MatchService matchService, 
    IMapper mapper) : ControllerBase
{
    [HttpPost("createMatch")]
    public async Task<IActionResult> MakePrediction(MatchCreationModel matchCreationModel)
    {
        await matchService.CreateMatch(mapper.Map<MatchCreationDto>(matchCreationModel));
        
        return Created();
    }

    [HttpGet("current")]
    public IActionResult GetWhetherUserHasCurrentMatch(string walletAddress)
    {
        return Ok(mapper.Map<CurrentMatchModel>(matchService.GetCurrentMatch(walletAddress)));
    }

    [HttpGet("history")]
    public IActionResult GetMatchHistory(string walletAddress, int offset, int limit)
    {
        return Ok(matchService.GetMatchHistory(walletAddress, offset, limit)
            .Select(match => mapper.Map<MatchModel>(match)));
    }
}