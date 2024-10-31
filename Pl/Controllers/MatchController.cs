using AutoMapper;
using Bll.Dtos;
using Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Filters.FilterAttributes;
using UpDownCryptorollBackend.Models;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("/match")]
[Authorize]
[UsernameAuthorization]
public class MatchController(
    MatchService matchService, 
    IMapper mapper) : ControllerBase
{
    [HttpPost("createMatch")]
    public async Task<IActionResult> MakePrediction(
        string username,
        [FromBody] MatchCreationModel matchCreationModel)
    {
        await matchService.CreateMatch(mapper.Map<MatchCreationDto>(matchCreationModel));
        
        return Created();
    }

    [HttpGet("current")]
    public IActionResult GetWhetherUserHasCurrentMatch(string username)
    {
        try
        {
            var matchDto = matchService.GetCurrentMatch(username);
            return Ok(mapper.Map<CurrentMatchModel>(matchDto));
        }
        catch (Exception e)
        {
            return Ok(new { Id = -1 });
        }
        
    }

    [HttpGet("history")]
    public IActionResult GetMatchHistory(string username, int offset, int limit)
    {
        return Ok(matchService.GetMatchHistory(username, offset, limit)
            .Select(match => mapper.Map<MatchModel>(match)));
    }
}