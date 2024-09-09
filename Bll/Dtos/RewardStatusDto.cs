namespace Bll.Dtos;

public class RewardStatusDto
{
    public string UserName { get; set; }
    public int LoginStreakCount { get; set; }
    public DateTimeOffset LastRewardedTime { get; set; }
    public DateTimeOffset LastLoginTime { get; set; }
    public bool IsRewardCollected { get; set; }
}