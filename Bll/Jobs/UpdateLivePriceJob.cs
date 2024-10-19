using Bll.Managers;
using Dal.DatabaseContext;
using Dal.Entities;
using Dal.Enums;
using Polly;
using Quartz;

namespace Bll.Jobs;

public class UpdateLivePriceJob(
    HttpClient httpClient,
    RegexManager regexManager,
    ApplicationDbContext applicationDbContext) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await UpdatePriceInDb(Coin.Btc);
        await UpdatePriceInDb(Coin.Eth);
        await UpdatePriceInDb(Coin.Ton);
    }
    
    private async Task<float> GetCurrentPrice(Coin coin)
    {
        try
        {
            var url = GetUrlForCoin(coin);
            
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(10, attempt => TimeSpan.FromSeconds(10 * Math.Pow(attempt, 2)));

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var htmlString = await response.Content.ReadAsStringAsync();

                var priceRegex = regexManager.GetRegexBasedOnCoinType(coin);
                var priceMatch = priceRegex.Match(htmlString);

                if (!priceMatch.Success) return -1;

                var priceString = priceMatch.Groups["price"].Value;

                if (float.TryParse(priceString.Replace(",", ""), out var parsedPrice))
                {
                    return parsedPrice;
                }

                return -1;
            });
        }
        catch (HttpRequestException e)
        {
            return -1;
        }
    }

    private async Task UpdatePriceInDb(Coin coinType)
    {
        var coinPrice = applicationDbContext.Prices.FirstOrDefault(c => c.Coin == coinType);
        
        if (coinPrice is null)
        {
            var newCoinPrice = new Price
            {
                Coin = coinType,
                Value = await GetCurrentPrice(coinType)
            };

            applicationDbContext.Prices.Add(newCoinPrice);

        }
        else
        {
            coinPrice.Value = await GetCurrentPrice(coinType);
        }

        await applicationDbContext.SaveChangesAsync();
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