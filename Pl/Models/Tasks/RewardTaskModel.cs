using Dal.Enums;

namespace UpDownCryptorollBackend.Models.Tasks;

public class RewardTaskModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Reward { get; set; }
    public RewardTaskStatus? Status { get; set; }   
}