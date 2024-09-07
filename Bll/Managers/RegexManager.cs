using System.Text.RegularExpressions;
using Dal.Enums;

namespace Bll.Managers;

public class RegexManager
{
    public Regex GetRegexBasedOnCoinType(Coin coin)
    {
        return coin switch
        {
            Coin.Btc => FindBitcoinPriceRegex(),
            Coin.Eth => FindEthereumPriceRegex(),
            Coin.Ton => FindTonPriceRegex(),
            _ => FindBitcoinPriceRegex(),
        };
    }

    private Regex FindBitcoinPriceRegex()
    {
        return new Regex("""content="[^"]*The live price of (?:Bitcoin|BTC) is \$(?<price>[\d,]+\.\d{2})""",
            RegexOptions.IgnoreCase);
    }

    private Regex FindEthereumPriceRegex()
    {
        return new Regex("""content="[^"]*The live price of (?:Ethereum|ETH) is \$(?<price>[\d,]+\.\d{2})""",
            RegexOptions.IgnoreCase);
    }

    private Regex FindTonPriceRegex()
    {
        return new Regex("""content="[^"]*The live price of (?:Toncoin|TON) is \$(?<price>[\d,]+\.\d{2})""",
            RegexOptions.IgnoreCase);

    }
}