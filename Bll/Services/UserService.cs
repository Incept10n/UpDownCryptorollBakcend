using AutoMapper;
using Bll.Dtos;
using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Entities;

namespace Bll.Services;

public class UserService(
    ApplicationDbContext applicationDbContext,
    RewardsService rewardsService,
    IMapper mapper)
{
    public UserDto GetUserInfo(string walletAddress)
    {
        var user =  applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);

        if (user is not null)
        {
            rewardsService.CalculateUserLoggedInReward(user);
            return mapper.Map<UserDto>(user);
        }
        
        var newUser = new User
        {
            WalletAddress = walletAddress,
            CurrentBalance = 10000,
            Name = "Unknown player",
            CurrentMatchId = null,
            
            LoginStreakCount = 1,
            LastRewardedTime = DateTimeOffset.Now.ToUniversalTime(),
            LastLoginTime = DateTimeOffset.Now.ToUniversalTime(),
            IsDailyRewardCollected = false,
        };

        applicationDbContext.Users.Add(newUser);
        applicationDbContext.SaveChanges();

        return mapper.Map<UserDto>(newUser);
    }

    public void ChangeUserName(string walletAddress, UserChangeNameDto userChangeNameDto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);

        if (user is null)
        {
            throw new UserNotFoundException($"user with wallet address: {walletAddress} was not found");
        }
        
        rewardsService.CalculateUserLoggedInReward(user);

        user.Name = userChangeNameDto.newName;

        applicationDbContext.SaveChanges();
    }
}