using AutoMapper;
using Bll.Dtos.Tasks;
using Bll.Services;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Models.Tasks;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("/tasks")]
public class TaskController(
    RewardTaskService rewardTaskService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllTasks(string walletAddress)
    {
        return Ok(rewardTaskService.GetAllTasks(walletAddress).Select(mapper.Map<RewardTaskDto>));
    }

    [HttpPost]
    public IActionResult ChangeTaskType(string walletAddress, TaskTypeChangeModel taskModel)
    {
        rewardTaskService.ChangeTaskStatus(walletAddress, mapper.Map<RewardTaskChangeDto>(taskModel));

        return Ok();
    }
}