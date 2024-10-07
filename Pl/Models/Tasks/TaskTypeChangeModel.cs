using Dal.Enums;

namespace UpDownCryptorollBackend.Models.Tasks;

public class TaskTypeChangeModel
{
    public int TaskId { get; set; }
    public RewardTaskStatus ChangedStatus { get; set; }
}