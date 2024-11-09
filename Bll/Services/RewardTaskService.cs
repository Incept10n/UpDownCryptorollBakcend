using Bll.Dtos.AdditionalInfo;
using Bll.Dtos.Tasks;
using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Entities;
using Dal.Entities.User;
using Dal.Enums;

namespace Bll.Services;

public class RewardTaskService(
    ApplicationDbContext applicationDbContext,
    UserService userService)
{
    public RewardTaskWithAdditionalInfo GetAllTasks(string username)
    {
        var tasks = applicationDbContext.RewardTasks.ToList();
        var user = userService.GetUserByName(username);
        
        var userTasks = applicationDbContext.UserTasks.FirstOrDefault(u => u.UserId == user.Id);

        if (userTasks is null)
        {
            var newUserTask = new UserTask
            {
                UserId = user.Id, 
                Completed = "",
                Uncollected = "",
                Uncompleted = "123456"
            };
            
            applicationDbContext.UserTasks.Add(newUserTask);
            
            userTasks = newUserTask;
        }

        applicationDbContext.SaveChanges();

        var userReferral = applicationDbContext.Referrals.FirstOrDefault(r => r.UserId == user.Id);
        var additionalInfo = new List<AdditionalInfoDto>();

        if (userReferral is null)
        {
            additionalInfo.Add(new AdditionalInfoDto
            {
                TaskId = 4,
                AdditionalInfo = new
                {
                    FriendsInvited = 0,
                }
            });
        }
        else
        {
            additionalInfo.Add(new AdditionalInfoDto
            {
                TaskId = 4,
                AdditionalInfo = new
                {
                    FriendsInvited = userReferral.FriendsInvited,
                }
            });
        }
        
        var tasksResponse= GetTasksWithStatuses(tasks, userTasks).OrderBy(task => task.Id).ToList();

        return new RewardTaskWithAdditionalInfo
        {
            Tasks = tasksResponse,
            AdditionalInfo = additionalInfo,
        };
    }

    public void ChangeTaskStatus(string username, RewardTaskChangeDto rewardTaskChangeDto)
    {
        var user = userService.GetUserByName(username);
        
        var userTasks = applicationDbContext.UserTasks.FirstOrDefault(u => u.UserId == user.Id);
        
        if (userTasks is null)
        {
            var newUserTask = new UserTask
            {
                UserId = user.Id, 
                Completed = "",
                Uncollected = "",
                Uncompleted = "123456"
            };
            
            applicationDbContext.UserTasks.Add(newUserTask);
            
            userTasks = newUserTask;
        }

        if (userTasks.Completed.Length + userTasks.Uncompleted.Length + userTasks.Uncollected.Length < rewardTaskChangeDto.TaskId)
        {
            throw new TaskOutOfRangeException($"task with id {rewardTaskChangeDto.TaskId} doesn't exist");
        }
        
        ChangeTaskStatus(user, userTasks, rewardTaskChangeDto);
        
        applicationDbContext.SaveChanges();
    }

    private List<RewardTaskDto> GetTasksWithStatuses(List<RewardTask> tasks, UserTask userTask)
    {
        List<RewardTaskDto> taskDtos = new List<RewardTaskDto>();
        
        foreach (var c in userTask.Uncompleted)
        {
            var taskId = int.Parse(c.ToString());
            var taskName = tasks.First(t => t.Id == taskId).Name;
            var taskReward = tasks.First(t => t.Id == taskId).Reward;
            var taskStatus = RewardTaskStatus.Uncompleted;

            taskDtos.Add(new RewardTaskDto {Id = taskId, Name = taskName, Reward = taskReward, Status = taskStatus});
        }
        
        foreach (var c in userTask.Uncollected)
        {
            var taskId = int.Parse(c.ToString());
            var taskName = tasks.First(t => t.Id == taskId).Name;
            var taskReward = tasks.First(t => t.Id == taskId).Reward;
            var taskStatus = RewardTaskStatus.Uncollected;
            
            taskDtos.Add(new RewardTaskDto {Id = taskId, Name = taskName, Reward = taskReward, Status = taskStatus});
        }
        
        foreach (var c in userTask.Completed)
        {
            var taskId = int.Parse(c.ToString());
            var taskName = tasks.First(t => t.Id == taskId).Name;
            var taskReward = tasks.First(t => t.Id == taskId).Reward;
            var taskStatus = RewardTaskStatus.Completed;
            
            taskDtos.Add(new RewardTaskDto {Id = taskId, Name = taskName, Reward = taskReward, Status = taskStatus});
        }
        
        return taskDtos;
    }

    private void ChangeTaskStatus(User user, UserTask userTasks, RewardTaskChangeDto rewardTaskChangeDto)
    {
        var taskIdChar = char.Parse(rewardTaskChangeDto.TaskId.ToString());
        
        userTasks.Completed = userTasks.Completed.Replace(taskIdChar.ToString(), "");
        userTasks.Uncollected = userTasks.Uncollected.Replace(taskIdChar.ToString(), "");
        userTasks.Uncompleted = userTasks.Uncompleted.Replace(taskIdChar.ToString(), "");

        switch (rewardTaskChangeDto.ChangedStatus)
        {
            case RewardTaskStatus.Completed:
                userTasks.Completed += taskIdChar.ToString();
                var task = applicationDbContext.RewardTasks.FirstOrDefault(task => task.Id == rewardTaskChangeDto.TaskId);
                user.CurrentBalance += task!.Reward;
                break;
            case RewardTaskStatus.Uncollected:
                userTasks.Uncollected += taskIdChar.ToString();
                break;
            case RewardTaskStatus.Uncompleted:
                userTasks.Uncompleted += taskIdChar.ToString();
                break;
        }
    }
}