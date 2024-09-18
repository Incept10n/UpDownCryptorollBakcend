using Dal.Enums;

namespace UpDownCryptorollBackend.Models;

public class CurrentMatchModel
{
    public int Id { get; set; }
    public float Bet { get; set; }
    public Coin Coin { get; set; }
    public Prediction Prediction { get; set; }
    public TimeSpan TimeRemaining { get; set; }
}