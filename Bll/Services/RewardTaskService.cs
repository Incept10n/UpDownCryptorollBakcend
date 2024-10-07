using Bll.Dtos.Tasks;
using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Entities;
using Dal.Enums;

namespace Bll.Services;

public class RewardTaskService(ApplicationDbContext applicationDbContext)
{
    public List<RewardTaskDto> GetAllTasks(string walletAddress)
    {
        var tasks = applicationDbContext.RewardTasks.ToList();
        var user = applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);
        
        if (user is null) throw new UserNotFoundException($"user with {walletAddress} not found");
        
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
        
        return GetTasksWithStatuses(tasks, userTasks).OrderBy(task => task.Id).ToList();
    }

    public void ChangeTaskStatus(string walletAddress, RewardTaskChangeDto rewardTaskChangeDto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);
        
        if (user is null) throw new UserNotFoundException($"user with {walletAddress} not found");
        
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
        
        ChangeTaskStatus(userTasks, rewardTaskChangeDto);
        
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

    private void ChangeTaskStatus(UserTask userTasks, RewardTaskChangeDto rewardTaskChangeDto)
    {
        var taskIdChar = char.Parse(rewardTaskChangeDto.TaskId.ToString());
        
        userTasks.Completed = userTasks.Completed.Replace(taskIdChar.ToString(), "");
        userTasks.Uncollected = userTasks.Uncollected.Replace(taskIdChar.ToString(), "");
        userTasks.Uncompleted = userTasks.Uncompleted.Replace(taskIdChar.ToString(), "");

        switch (rewardTaskChangeDto.ChangedStatus)
        {
            case RewardTaskStatus.Completed:
                userTasks.Completed += taskIdChar.ToString();
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