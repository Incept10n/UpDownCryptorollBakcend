using Bll.Managers;
using Dal.Enums;

namespace Bll.Services;

public class CurrentPriceService(
    HttpClient httpClient, 
    RegexManager regexManager)
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