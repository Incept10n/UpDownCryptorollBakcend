using System.Reflection.Metadata.Ecma335;
using Dal.Enums;

namespace Bll.Dtos;

public class MatchCreationDto
{
    public string Username { get; set; }
    public Coin Coin { get; set; }
    public float PredictionAmount { get; set; }
    public TimeSpan PredictionTimeframe { get; set; }
    public Prediction PredictionValue { get; set; }
    
}