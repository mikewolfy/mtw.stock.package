using Emptywolf.Stocks;
using System;
using Xunit;

namespace mtw.stock.package.tests
{
    public class MapperTests
    {
        [Fact]
        public void MapIexResponseToStockSuccess()
        {
            //setup
            var iexResponse = new IexResponse
            {
                quote = new IexResponseQuote
                {
                    symbol = "AAPL",
                    change = null,
                    companyName = "Apple",
                    latestPrice = 215.5050M,
                    open = 210.75M,
                    peRatio = 30.45M,
                    week52High = 250.10M,
                    week52Low = 160.75M,
                    changePercent = 0.20M,
                    sector = "Technology"
                }
            };
            var mapper = new Mapper();

            //execute
            var stock = mapper.MapIexResponseToStock(iexResponse);

            //validate
            Assert.Equal(iexResponse.quote.symbol, stock.Ticker);
            Assert.Equal(iexResponse.quote.companyName, stock.Company);
            Assert.Equal(iexResponse.quote.sector, stock.Sector);
            Assert.Equal(Math.Round(iexResponse.quote.latestPrice.Value, 2), stock.Price);
            Assert.Equal(iexResponse.quote.week52High, stock.Week52High);
            Assert.Equal(iexResponse.quote.week52Low, stock.Week52Low);
            Assert.False(stock.DailyPercentageChange == 0);
            Assert.False(stock.DailyChange == 0);
            Assert.True(DateTime.UtcNow.Subtract(stock.LastUpdated).TotalSeconds < 20);
        }
    }
}