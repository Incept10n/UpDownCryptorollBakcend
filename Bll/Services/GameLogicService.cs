using Bll.Dtos;
using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Entities;
using Dal.Enums;

namespace Bll.Services;

public class GameLogicService(
    ApplicationDbContext applicationDbContext,
    JobScheduleService jobScheduleService,
    CurrentPriceService currentPriceService)
{
    public async Task CreateMatch(MatchCreationDto matchCreationDto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user =>
            user.WalletAddress == matchCreationDto.WalletAddress);

        if (user is null)
        {
            throw new UserNotFoundException($"user with wallet address {matchCreationDto.WalletAddress} was not found");
        }

        if (matchCreationDto.PredictionAmount <= 0
            || user.CurrentBalance - matchCreationDto.PredictionAmount < 0)
        {
            throw new InvalidBetAmountException($"your bet of {matchCreationDto.PredictionAmount} is invalid");
        }
        
        ValidateDataForMatch(matchCreationDto);
        
        var match = new Match
        {
            User = user,
            Coin = matchCreationDto.Coin,
        
            EntryTime = DateTimeOffset.Now.ToUniversalTime(),
            EntryPrice = currentPriceService.GetCurrentPrice(Coin.Btc),
        
            Prediction = matchCreationDto.PredictionValue,
            PredictionTimeframe = matchCreationDto.PredictionTimeframe,
            PredictionAmount = matchCreationDto.PredictionAmount,
        
            ExitTime = null,
            ExitPrice = null,
        
            Res = null,
            ResultPayout = null,
        };
        
        applicationDbContext.Matches.Add(match);
        await applicationDbContext.SaveChangesAsync();
        
        user.CurrentMatchId = match.Id;
        await applicationDbContext.SaveChangesAsync();

        // this decides the result of a job after some delay
        await jobScheduleService.CompleteMatchResult(match.Id, match.PredictionTimeframe);
    }

    private void ValidateDataForMatch(MatchCreationDto matchCreationDto)
    {
        if (matchCreationDto.PredictionTimeframe != TimeSpan.FromSeconds(15)
            && matchCreationDto.PredictionTimeframe != TimeSpan.FromMinutes(30)
            && matchCreationDto.PredictionTimeframe != TimeSpan.FromHours(4)
            && matchCreationDto.PredictionTimeframe != TimeSpan.FromHours(12))
        {
            throw new WrongPredictionTimeframeException(
                $"timeframe of {matchCreationDto.PredictionTimeframe.TotalSeconds} seconds is not valid");
        }
    }
}