using AutoMapper;
using Bll.Dtos;
using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Entities;
using Dal.Enums;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Bll.Services;

public class MatchService(
    ApplicationDbContext applicationDbContext,
    JobScheduleService jobScheduleService,
    CurrentPriceService currentPriceService,
    IMapper mapper)
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
            throw new InvalidBetAmountException(
                $"insufficient funds, you have {user.CurrentBalance} but tried to bet {matchCreationDto.PredictionAmount}");
        }

        if (user.CurrentMatchId is not null)
        {
            throw new UserAlreadyInMatchException($"user with id {user.Id} is already in match");
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
        user.CurrentBalance -= match.PredictionAmount;
        await applicationDbContext.SaveChangesAsync();
        
        user.CurrentMatchId = match.Id;
        await applicationDbContext.SaveChangesAsync();

        // this decides the result of a job after some delay
        await jobScheduleService.CompleteMatchResult(match.Id, match.PredictionTimeframe);
    }

    public List<MatchDto> GetMatchHistory(string walletAddress, int offset, int limit)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);

        if (user is null) throw new UserNotFoundException($"user with wallet address: {walletAddress} was not found");

        return applicationDbContext.Matches
            .OrderBy(match => match.EntryTime)
            .Skip(offset)
            .Take(limit)
            .Select(match => mapper.Map<MatchDto>(match))
            .ToList();
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