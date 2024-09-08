using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Enums;

namespace Bll.Services;

public class CurrentPriceService(ApplicationDbContext applicationDbContext)
{
    
    public float GetCurrentPrice(Coin coin)
    {
        var coinPrice = applicationDbContext.Prices.FirstOrDefault(p => p.Coin == coin);

        if (coinPrice is null)
        {
            throw new UnknownCoinTypeException($"the coin of type {coin} does not exist");
        }

        return coinPrice.Value;
    }
}