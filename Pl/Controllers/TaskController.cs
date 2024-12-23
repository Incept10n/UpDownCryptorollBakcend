using AutoMapper;
using Bll.Dtos.Tasks;
using Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Filters.FilterAttributes;
using UpDownCryptorollBackend.Models.Tasks;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
[UsernameAuthorization]
public class TaskController(
    RewardTaskService rewardTaskService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllTasks(string username)
    {
        return Ok(rewardTaskService.GetAllTasks(username));
    }

    [HttpPost]
    public IActionResult ChangeTaskType(string username, TaskTypeChangeModel taskModel)
    {
        rewardTaskService.ChangeTaskStatus(username, mapper.Map<RewardTaskChangeDto>(taskModel));

        return Ok();
    }
}