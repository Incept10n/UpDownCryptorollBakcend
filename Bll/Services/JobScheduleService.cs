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
            .StartAt(DateBuilder.FutureDate((int)predictionTimeframe.TotalSeconds, IntervalUnit.Second))
            .Build();
        
        scheduler.ScheduleJob(jobDetail, trigger);
    }

    public async Task SetUpdatingLivePrice()
    {
        var scheduler = await schedulerFactory.GetScheduler();
        
        var currentPriceUpdateJob = JobBuilder.Create<UpdateLivePriceJob>()
            .WithIdentity("livePriceUpdater", "prices")
            .Build();

        var currentPriceUpdateTrigger = TriggerBuilder.Create()
            .WithIdentity("livePriceTrigger", "prices")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
            .Build();

        scheduler.ScheduleJob(currentPriceUpdateJob, currentPriceUpdateTrigger);
    }
}