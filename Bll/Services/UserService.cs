using AutoMapper;
using Bll.Dtos;
using Bll.Dtos.Users;
using Bll.Exceptions;
using Bll.Managers;
using Dal.DatabaseContext;
using Dal.Entities.User;

namespace Bll.Services;

public class UserService(
    ApplicationDbContext applicationDbContext,
    RewardsService rewardsService,
    MatchService matchService,
    JwtTokenManager jwtTokenManager,
    IMapper mapper)
{
    public UserDto GetUserInfo(string username)
    {
        var user = GetUserByName(username);
        
        rewardsService.CalculateUserLoggedInReward(user);
        
        return mapper.Map<UserDto>(user);
    }

    public UserWithJwtTokenDto CreateUser(UserCreationDto userCreationDto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Name == userCreationDto.Username);

        if (user is not null)
            throw new UserAlreadyExistsException($"user with name {userCreationDto.Username} already exists");
        
        var newUser = CreateNewDefaultUser(userCreationDto);
        
        applicationDbContext.Users.Add(newUser);
        applicationDbContext.SaveChanges();
        
        string token = jwtTokenManager.GenerateJwtToken(newUser);
        return new UserWithJwtTokenDto
        {
            UserDto = mapper.Map<UserDto>(newUser),
            JwtToken = token,
        };
    }

    public void ChangeUserInfo(string currentUsername, UserChangeInfoDto userChangeInfoDto)
    {
        var user = GetUserByName(currentUsername);
        
        rewardsService.CalculateUserLoggedInReward(user);

        user.Name = userChangeInfoDto.Name ?? user.Name;
        user.Password = BCrypt.Net.BCrypt.HashPassword(userChangeInfoDto.Password) ?? user.Password;
        user.WalletAddress = userChangeInfoDto.WalletAddress ?? user.WalletAddress;

        applicationDbContext.SaveChanges();
    }
    
    public void CollectMatch(string username)
    {
        var user = GetUserByName(username);

        // get the latest match
        var match = matchService.GetMatchHistory(username, 0, 1).First();

        user.IsLastMatchCollected = true;
        user.CurrentBalance += match.ResultPayout;

        applicationDbContext.SaveChanges();
    }

    public UserWithJwtTokenDto Login(UserCreationDto userCreationDto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Name == userCreationDto.Username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(userCreationDto.Password, user.Password))
        {
            throw new IncorrectUsernameOrPassword($"incorrect username or password");
        }
        
        string token = jwtTokenManager.GenerateJwtToken(user);
        
        return new UserWithJwtTokenDto
        {
            UserDto = mapper.Map<UserDto>(user),
            JwtToken = token,
        };
    }

    private User CreateNewDefaultUser(UserCreationDto userCreationDto)
    {
        return new User
        {
            Name = userCreationDto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(userCreationDto.Password),
            WalletAddress = string.Empty,
                           
            CurrentBalance = 10000,
            CurrentMatchId = null,
                           
            LoginStreakCount = 1,
            LastRewardedTime = DateTimeOffset.Now.ToUniversalTime(),
            LastLoginTime = DateTimeOffset.Now.ToUniversalTime(),
            IsDailyRewardCollected = false,
            IsLastMatchCollected = true,
        };
    }

    /// <summary>
    /// Returns user by name
    /// </summary>
    /// <param name="name">User name</param>
    /// <returns>User entity</returns>
    /// <exception cref="UserNotFoundException">Throws if user with an entered name doesn't exist</exception>
    public User GetUserByName(string name)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Name == name);

        if (user is null) 
            throw new UserNotFoundException($"user with name {name} not found");

        return user;
    }
}