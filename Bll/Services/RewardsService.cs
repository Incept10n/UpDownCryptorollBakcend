using Bll.Dtos;
using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Entities.User;

namespace Bll.Services;

public class RewardsService(ApplicationDbContext applicationDbContext)
{
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
    
    public RewardStatusDto GetRewardStatus(string username)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Name == username);

        if (user is null)
        {
            throw new UserNotFoundException($"user with name: {username} was not found");
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

    public void CollectReward(string username)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Name == username);

        if (user is null)
        {
            throw new UserNotFoundException($"user with name: {username} was not found");
        }

        user.IsDailyRewardCollected = true;
        
        user.CurrentBalance += CalculateDailyReward(user.LoginStreakCount);

        applicationDbContext.SaveChanges();
    }

    private int CalculateDailyReward(int loginStreakCount)
    {
        return 200 + (loginStreakCount - 1) * 60;
    }
}