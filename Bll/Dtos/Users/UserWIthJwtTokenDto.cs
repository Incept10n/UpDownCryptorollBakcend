namespace Bll.Dtos.Users;

public class UserWithJwtTokenDto
{
    public UserDto UserDto { get; set; }
    public string JwtToken { get; set; }
}