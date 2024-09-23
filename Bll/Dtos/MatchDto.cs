using Dal.Enums;

namespace Bll.Dtos;

public class MatchDto
{
    public int Id { get; set; }
    public float Bet { get; set; }
    public Coin Coin { get; set; }
    
    public DateTimeOffset EntryDateTime { get; set; }
    public float EntryPrice { get; set; }
    
    public DateTimeOffset ExitDateTIme { get; set; }
    public float ExitPrice { get; set; }
    
    public Prediction PredictionValue { get; set; }
    public TimeSpan PredictionTimeframe { get; set; }

    public ResultStatus ResultStatus { get; set; }
    public int ResultPayout { get; set; }
}