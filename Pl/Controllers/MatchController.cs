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
    IMapper mapper) : ControllerBase
{
    [HttpPost("createMatch")]
    public async Task<IActionResult> MakePrediction(MatchCreationModel matchCreationModel)
    {
        await gameLogicService.CreateMatch(mapper.Map<MatchCreationDto>(matchCreationModel));
        
        return Created();
    }
}