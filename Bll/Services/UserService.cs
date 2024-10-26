using AutoMapper;
using Bll.Dtos;
using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Entities.User;

namespace Bll.Services;

public class UserService(
    ApplicationDbContext applicationDbContext,
    RewardsService rewardsService,
    MatchService matchService,
    IMapper mapper)
{
    public UserDto GetUserInfo(string username)
    {
        var user =  applicationDbContext.Users.FirstOrDefault(user => user.Name == username);

        if (user is null)
        {
            throw new UserNotFoundException($"user with username {username} not found");
        }
        
        rewardsService.CalculateUserLoggedInReward(user);
        return mapper.Map<UserDto>(user);
    }

    public void CreateUser(UserCreationDto userCreationDto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Name == userCreationDto.Username);

        if (user is not null)
        {
            throw new UserAlreadyExistsException($"user with name {userCreationDto.Username} already exists");
        }
        
        var newUser = CreateNewDefaultUser(userCreationDto);
        
        applicationDbContext.Users.Add(newUser);
        applicationDbContext.SaveChanges();
    }

    public void ChangeUserInfo(string currentUsername, UserChangeInfoDto userChangeInfoDto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Name == currentUsername);

        if (user is null)
        {
            throw new UserNotFoundException($"user with name {currentUsername} not found");
        }
        
        rewardsService.CalculateUserLoggedInReward(user);

        if (userChangeInfoDto.Name is not null)
        {
            var searchUser = applicationDbContext.Users.FirstOrDefault(u => u.Name == userChangeInfoDto.Name);
            
            if (searchUser is not null) throw new UserAlreadyExistsException($"user with name {userChangeInfoDto.Name} already exists");
            
            user.Name = userChangeInfoDto.Name ?? user.Name;
        }

        if (userChangeInfoDto.WalletAddress is not null)
        {
            var searchUser = applicationDbContext.Users.FirstOrDefault(u => u.WalletAddress == userChangeInfoDto.WalletAddress);
            
            if (searchUser is not null) throw new UserAlreadyExistsException($"user with name {userChangeInfoDto.Name} already exists");
            
            user.WalletAddress = userChangeInfoDto.WalletAddress ?? user.WalletAddress;
        }

        applicationDbContext.SaveChanges();
    }
    
    public void CollectMatch(string username)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Name == username);

        if (user is null) throw new UserNotFoundException($"user with username {username} not found");

        // get the latest match
        var match = matchService.GetMatchHistory(username, 0, 1).First();

        user.IsLastMatchCollected = true;
        user.CurrentBalance += match.ResultPayout;

        applicationDbContext.SaveChanges();
    }

    private User CreateNewDefaultUser(UserCreationDto userCreationDto)
    {
        return new User
        {
            Name = userCreationDto.Username,
            Password = userCreationDto.Password,
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
}