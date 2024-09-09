using AutoMapper;
using Bll.Dtos;
using Bll.Services;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Models;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("/match")]
public class MatchController(
    GameLogicService gameLogicService, 
    IMapper mapper,
    UserService userService) : ControllerBase
{
    [HttpPost("createMatch")]
    public async Task<IActionResult> MakePrediction(MatchCreationModel matchCreationModel)
    {
        await gameLogicService.CreateMatch(mapper.Map<MatchCreationDto>(matchCreationModel));
        
        return Created();
    }

    [HttpGet("current")]
    public IActionResult GetWhetherUserHasCurrentMatch(string walletAddress)
    {
        return Ok(new { inMatch = userService.IsCurrentlyInMatch(walletAddress) });
    }
}