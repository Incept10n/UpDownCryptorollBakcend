using Bll.Constants;
using Bll.Exceptions;
using Bll.Services;
using Dal.DatabaseContext;
using Dal.Entities;
using Dal.Enums;
using Quartz;

namespace Bll.Jobs;

public class SetMatchResultJob(
    ApplicationDbContext applicationDbContext,
    CurrentPriceService currentPriceService) : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var matchId = (int)context.MergedJobDataMap["matchId"];
        
        var match = applicationDbContext.Matches.FirstOrDefault(match => match.Id == matchId);

        if (match is null)
        {
            throw new MatchNotFoundException($"the match with id {matchId} was not found");
        }

        var currentExitPrice = currentPriceService.GetCurrentPrice(match.Coin);
        
        match.ExitTime = DateTimeOffset.Now.ToUniversalTime();
        match.ExitPrice = currentExitPrice;
        match.Res = GetMatchResult(match, currentExitPrice);
        match.ResultPayout = GetMatchResultPayout(match);

        applicationDbContext.SaveChanges();
        
        GiveUserPayout(match);
        
        return Task.CompletedTask;
    }

    private ResultStatus GetMatchResult(Match match, float currentExitPrice)
    {
        return (currentExitPrice > match.EntryPrice) ? ResultStatus.Win : ResultStatus.Loss;
    }

    private float GetMatchResultPayout(Match match)
    {
        if (match.Res != ResultStatus.Win) return 0 - match.PredictionAmount;

        if (match.PredictionTimeframe == TimeSpan.FromSeconds(15))
            return match.PredictionAmount * 2f - match.PredictionAmount;
        if (match.PredictionTimeframe == TimeSpan.FromMinutes(30))
            return match.PredictionAmount * TimeFrameMultiplier.ThirtyMinutesMultiplier - match.PredictionAmount;
        if (match.PredictionTimeframe == TimeSpan.FromHours(4)) 
            return match.PredictionAmount * TimeFrameMultiplier.FourHoursMultiplier - match.PredictionAmount;
        if (match.PredictionTimeframe == TimeSpan.FromHours(12)) 
            return match.PredictionAmount * TimeFrameMultiplier.TwelveHoursMultiplier - match.PredictionAmount;

        return match.PredictionAmount * 0f;
    }

    private void GiveUserPayout(Match match)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.Id == match.UserId);

        if (user is null)
        {
            throw new UserNotFoundException($"the user with id {match.UserId} was not found");
        }

        if (user.CurrentBalance + match.ResultPayout < 0)
        {
            throw new InvalidBetAmountException(
                $"the bet with amount {match.PredictionAmount} was not valid, " +
                "match results have no effect");
        }

        user.CurrentBalance += match.ResultPayout
                               ?? throw new InvalidBetAmountException("bet resulted in null payout");

        user.CurrentMatchId = null;
        
        applicationDbContext.SaveChanges();
    }
}