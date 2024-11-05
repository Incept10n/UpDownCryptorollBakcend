using AutoMapper;
using Bll.Dtos;
using Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Filters.FilterAttributes;
using UpDownCryptorollBackend.Models.Users;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("/user")]
[Authorize]
[UsernameAuthorization]
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
        var newToken = userService.ChangeUserInfo(username, mapper.Map<UserChangeInfoDto>(userChangeInfoModel));

        return Ok(new { NewToken = newToken });
    }

    [HttpPost("collectLastMatch")]
    public IActionResult CollectLastMatch([FromQuery] string username)
    {
        userService.CollectMatch(username);

        return Ok();
    }
}