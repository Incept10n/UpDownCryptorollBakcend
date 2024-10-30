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
        userService.CreateUser(mapper.Map<UserCreationDto>(userCreationModel));

        return Created();
    }

    [HttpPost("login")]
    public Task<IActionResult> Login([FromBody] UserCreationModel userCreationModel)
    {
        
    }
}