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

        var entryPrice = await currentPriceService.GetCurrentPrice(Coin.Btc);

        var match = new Match
        {
            User = user,
            Coin = matchCreationDto.Coin,
        
            EntryTime = DateTimeOffset.Now.ToUniversalTime(),
            EntryPrice = entryPrice,
        
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
        
        await jobScheduleService.CompleteMatchResult(match.Id, TimeSpan.FromSeconds(15));
    }
    
}