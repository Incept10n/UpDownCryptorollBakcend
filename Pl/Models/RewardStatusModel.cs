namespace UpDownCryptorollBackend.Models;

public class RewardStatusModel
{
    public string UserName { get; set; }
    public int LoginStreakCount { get; set; }
    public DateTimeOffset LastRewardedTime { get; set; }
    public DateTimeOffset LastLoginTime { get; set; }
    public bool isRewardCollected { get; set; }
}