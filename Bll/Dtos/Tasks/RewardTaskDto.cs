using Dal.Enums;

namespace Bll.Dtos.Tasks;

public class RewardTaskDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Reward { get; set; }
    public RewardTaskStatus? Status { get; set; }
}