using Bll.Jobs;
using Quartz;

namespace Bll.Services;

public class JobScheduleService(ISchedulerFactory schedulerFactory)
{
    public async Task CompleteMatchResult(int matchId, TimeSpan predictionTimeframe)
    {
        var scheduler = await schedulerFactory.GetScheduler();
        
        var jobDetail = JobBuilder
            .Create<SetMatchResultJob>()
            .WithIdentity($"match{matchId}ResultJob", "resultMatchmaking")
            .UsingJobData("matchId", matchId)
            .Build();
        
        var trigger = TriggerBuilder.Create()
            .WithIdentity($"match{matchId}ResultTrigger", "resultMatchmaking")
            .StartAt(DateBuilder.FutureDate(predictionTimeframe.Seconds, IntervalUnit.Second))
            .Build();
        
        scheduler.ScheduleJob(jobDetail, trigger);
    }
}