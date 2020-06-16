using System;

namespace Emptywolf.Stocks
{
    public class Mapper: IMapper
    {
        public Stock MapIexResponseToStock(IexResponse response)
        {
            if (response == null)
            {
                return null;
            }

            decimal price = response.quote.latestPrice.HasValue ? response.quote.latestPrice.Value : 0;
            decimal open = response.quote.open.HasValue ? response.quote.open.Value : 0;
            decimal close = response.quote.close.HasValue ? response.quote.close.Value : 0;
            decimal change = price - open;
            decimal percentageChange = price == 0 ? 0 : change / price * 100;
            decimal changeSinceClose = price - close;
            decimal percentageChangeSinceClose = price == 0 ? 0 : changeSinceClose / price * 100;

            var stock = new Stock()
            {
                Ticker = response.quote.symbol,
                Company = response.quote.companyName,
                Price = Math.Round(price, 2),
                PE = response.quote.peRatio.HasValue ? response.quote.peRatio.Value : 0,
                Week52High = response.quote.week52High.HasValue ? Math.Round(response.quote.week52High.Value, 2) : 0,
                Week52Low = response.quote.week52Low.HasValue ? Math.Round(response.quote.week52Low.Value, 2) : 0,
                Sector = response.quote.sector,
                DailyChange = Math.Round(change, 2),
                DailyPercentageChange = Math.Round(percentageChange, 2),
                DailyChangeSinceLastClose = Math.Round(changeSinceClose, 2),
                DailyPercentageChangeSinceLastClose = Math.Round(percentageChangeSinceClose, 2),
                YearToDateChange = response.quote.ytdChange.HasValue ? Math.Round(response.quote.ytdChange.Value, 4) : 0,
                LastUpdated = DateTime.UtcNow
            };
            stock.Eps = stock.PE != 0 ? stock.Price / stock.PE : 0;
            return stock;
        }
    }
}