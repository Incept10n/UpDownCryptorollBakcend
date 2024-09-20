using Dal.Enums;

namespace Bll.Dtos;

public class CurrentMatchDto
{
    public int Id { get; set; }
    public float Bet { get; set; }
    public Coin Coin { get; set; }
    public Prediction Prediction { get; set; }
    public TimeSpan TimeRemaining { get; set; }
    public float WinningMultiplier { get; set; }
    public float EntryPrice { get; set; }
}