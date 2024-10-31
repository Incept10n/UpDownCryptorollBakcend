using AutoMapper;
using Bll.Dtos;
using Bll.Services;
using Dal.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Models;
using UpDownCryptorollBackend.Models.Users;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("/user")]
[Authorize]
public class UserController(
    UserService userService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetUserInfo(string username)
    {
        var user = userService.GetUserInfo(username);

        return Ok(mapper.Map<UserModel>(user));
    }

    [HttpPut]
    public IActionResult ChangeUserInfo(string username, UserChangeInfoModel userChangeInfoModel)
    {
        userService.ChangeUserInfo(username, mapper.Map<UserChangeInfoDto>(userChangeInfoModel));

        return Ok();
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] UserCreationModel userCreationModel)
    {
        userService.CreateUser(mapper.Map<UserCreationDto>(userCreationModel));

        return Created();
    }

    [HttpPost("collectLastMatch")]
    public IActionResult CollectLastMatch([FromQuery] string walletAddress)
    {
        userService.CollectMatch(walletAddress);

        return Ok();
    }
}