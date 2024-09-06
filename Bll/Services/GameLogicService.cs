using Bll.Dtos;
using Bll.Exceptions;
using Bll.Managers;
using Dal.DatabaseContext;
using Dal.Entities;
using Dal.Enums;

namespace Bll.Services;

public class GameLogicService(
    HttpClient httpClient,
    RegexManager regexManager,
    ApplicationDbContext applicationDbContext)
{
    public async Task<float> GetCurrentPrice(Coin coin)
    {
        try
        {
            var url = GetUrlForCoin(coin);
            
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); 
            
            var htmlString = await response.Content.ReadAsStringAsync();

            var priceRegex = regexManager.GetRegexBasedOnCoinType(coin);
            var priceMatch = priceRegex.Match(htmlString);

            if (!priceMatch.Success) return -1;
            
            var priceString = priceMatch.Groups["price"].Value;
            
            if (float.TryParse(priceString.Replace(",", ""), out var bitcoinPrice))
            {
                return bitcoinPrice;
            }

            return -1;
        }
        catch (HttpRequestException e)
        {
            return -1;
        }
    }

    public void CreateMatch(MatchCreationDto matchCreationDto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user =>
            user.WalletAddress == matchCreationDto.WalletAddress);

        if (user is null)
        {
            throw new UserNotFoundException($"user with wallet address {matchCreationDto.WalletAddress} was not found");
        }

        var match = new Match
        {
            User = user,
            Coin = matchCreationDto.Coin,

            EntryTime = DateTimeOffset.Now.ToUniversalTime(),
            EntryPrice = matchCreationDto.EntryPrice,

            Prediction = matchCreationDto.PredictionValue,
            PredictionTimeframe = matchCreationDto.PredictionTimeframe,
            PredictionAmount = matchCreationDto.PredictionAmount,

            ExitTime = null,
            ExitPrice = null,

            Res = null,
            ResultPayout = null,
        };

        applicationDbContext.Matches.Add(match);

        applicationDbContext.SaveChanges();
    }
    
    private string GetUrlForCoin(Coin coin)
    {
        return coin switch
        {
            Coin.Btc => "https://www.okx.com/price/bitcoin-btc",
            Coin.Eth => "https://www.okx.com/price/ethereum-eth",
            Coin.Ton => "https://www.okx.com/price/toncoin-ton",
            _ => throw new ArgumentException("Unsupported coin type")
        };
    }
}