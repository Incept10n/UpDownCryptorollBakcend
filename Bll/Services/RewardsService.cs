using Bll.Dtos;
using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Entities;
using Dal.Entities.User;

namespace Bll.Services;

public class RewardsService(ApplicationDbContext applicationDbContext)
{
    public void CalculateUserLoggedInReward(string walletAddress)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);

        if (user is null)
        {
            throw new UserNotFoundException($"user with wallet address: {walletAddress} was not found");
        }

        var newloginTime = DateTimeOffset.Now.ToUniversalTime();

        user.LastLoginTime = newloginTime;

        if ((user.LastLoginTime - user.LastRewardedTime) > TimeSpan.FromHours(48))
        {
            user.LastRewardedTime = newloginTime;
            user.LoginStreakCount = 1;
            user.IsDailyRewardCollected = false;
        }
        else if (user.LastLoginTime - user.LastRewardedTime > TimeSpan.FromHours(24)
                 && user.LastLoginTime - user.LastRewardedTime < TimeSpan.FromHours(48))
        {
            user.LastRewardedTime = newloginTime;
            user.LoginStreakCount += 1;
            user.IsDailyRewardCollected = false;
        }
        else
        {
            user.LastRewardedTime = newloginTime;
        }

        applicationDbContext.SaveChanges();
    }
    
    
    public void CalculateUserLoggedInReward(User user)
    {
        var newloginTime = DateTimeOffset.Now.ToUniversalTime();

        user.LastLoginTime = newloginTime;

        if ((user.LastLoginTime - user.LastRewardedTime) > TimeSpan.FromHours(48))
        {
            user.LastRewardedTime = newloginTime;
            user.LoginStreakCount = 1;
            user.IsDailyRewardCollected = false;
        }
        else if (user.LastLoginTime - user.LastRewardedTime > TimeSpan.FromHours(24)
                 && user.LastLoginTime - user.LastRewardedTime < TimeSpan.FromHours(48))
        {
            user.LastRewardedTime = newloginTime;
            user.LoginStreakCount += 1;
            user.IsDailyRewardCollected = false;
        }
        
        applicationDbContext.SaveChanges();
    }
    
    public RewardStatusDto GetRewardStatus(string walletAddress)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);

        if (user is null)
        {
            throw new UserNotFoundException($"user with wallet address: {walletAddress} was not found");
        }
        
        CalculateUserLoggedInReward(user);

        return new RewardStatusDto
        {
            UserName = user.Name,
            LoginStreakCount = user.LoginStreakCount,
            LastRewardedTime = user.LastRewardedTime,
            LastLoginTime = user.LastLoginTime,
            IsRewardCollected = user.IsDailyRewardCollected,
        };
    }

    public void CollectReward(string walletAddress)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);

        if (user is null)
        {
            throw new UserNotFoundException($"user with wallet address: {walletAddress} was not found");
        }

        user.IsDailyRewardCollected = true;

        applicationDbContext.SaveChanges();
    }
}