using AutoMapper;
using Bll.Dtos;
using Bll.Services;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Models;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("/user")]
public class UserController(
    UserService userService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetUserInfo(string walletAddress)
    {
        var user = userService.GetUserInfo(walletAddress);

        return Ok(mapper.Map<UserModel>(user));
    }

    [HttpPut]
    public IActionResult ChangeUserName(string walletAddress, UserChangeNameModel userChangeNameModel)
    {
        userService.ChangeUserName(walletAddress, mapper.Map<UserChangeNameDto>(userChangeNameModel));

        return Ok();
    }

    [HttpPost("collectLastMatch")]
    public IActionResult CollectLastMatch([FromQuery] string walletAddress)
    {
        userService.CollectMatch(walletAddress);

        return Ok();
    }
}