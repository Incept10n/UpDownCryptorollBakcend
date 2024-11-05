namespace Bll.Dtos.Users;

public class UserWithJwtTokenDto
{
    public UserDto User { get; set; }
    public string JwtToken { get; set; }
}