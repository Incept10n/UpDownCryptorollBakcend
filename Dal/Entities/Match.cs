using Dal.Enums;

namespace Dal.Entities;

public class Match
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public Coin Coin { get; set; }
    
    public DateTimeOffset EntryTime { get; set; }
    public float EntryPrice { get; set; }
    
    public Prediction Prediction { get; set; }
    public TimeSpan PredictionTimeframe { get; set; }
    public float PredictionAmount { get; set; }
    
    public DateTimeOffset? ExitTime { get; set; }
    public float? ExitPrice { get; set; }

    public ResultStatus? Res { get; set; }
    public float? ResultPayout { get; set; }
}