using Dal.Enums;

namespace UpDownCryptorollBackend.Models;

public class MatchCreationModel
{
    public string Username { get; set; }
    public Coin Coin { get; set; }
    public float PredictionAmount { get; set; }
    public TimeSpan PredictionTimeframe { get; set; }
    public Prediction PredictionValue { get; set; }
}