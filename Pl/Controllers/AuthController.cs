using AutoMapper;
using Bll.Services;
using Dal.Entities.User;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Models.Users;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
public class AuthController(
    UserService userService,
    IMapper mapper) : ControllerBase
{
    [HttpPost("signup")]
    public IActionResult Signup([FromBody] UserCreationModel userCreationModel)
    {
        return Created("/signup", userService.CreateUser(mapper.Map<UserCreationDto>(userCreationModel)));
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserCreationModel userCreationModel)
    {
        return Created("/login", userService.Login(mapper.Map<UserCreationDto>(userCreationModel)));
    }
}