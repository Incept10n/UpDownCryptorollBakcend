using Bll.Dtos.AdditionalInfo;

namespace Bll.Dtos.Tasks;

public class RewardTaskWithAdditionalInfo
{
    public List<RewardTaskDto> Tasks { get; set; }
    public List<AdditionalInfoDto> AdditionalInfo { get; set; }
}