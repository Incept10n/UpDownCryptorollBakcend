namespace Dal.Entities;

public class User
{
    public int Id { get; set; }
    public string WalletAddress { get; set; }
    public string Name { get; set; }
    public float CurrentBalance { get; set; }
    public int? CurrentMatchId { get; set; }
    
    public int LoginStreakCount { get; set; }
    public DateTimeOffset LastRewardedTime { get; set; }
    public DateTimeOffset LastLoginTime { get; set; }
    public bool IsDailyRewardCollected { get; set; }
}