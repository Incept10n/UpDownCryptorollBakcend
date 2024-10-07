using Dal.Enums;

namespace Bll.Dtos.Tasks;

public class RewardTaskChangeDto
{
    public int TaskId { get; set; }
    public RewardTaskStatus ChangedStatus { get; set; }
}