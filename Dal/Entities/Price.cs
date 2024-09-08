using Dal.Enums;

namespace Dal.Entities;

public class Price
{
    public int Id { get; set; }
    public Coin Coin { get; set; }
    public float Value { get; set; }
}