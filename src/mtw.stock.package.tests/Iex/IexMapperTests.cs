using Emptywolf.Stocks;
using Emptywolf.Stocks.Iex.Models;
using System;
using Xunit;

namespace mtw.stock.package.tests.Iex
{
    public class IexMapperTests
    {
        [Theory]
        [InlineData(215.5050, 210.6050, "tops", 215.5050, false)]
        [InlineData(215.5050, 210.6050, "previousclose", 210.6050, true)]
        public void MapIexResponseToStockSuccess(decimal latestPrice, decimal close, string calculationType, decimal price, bool dailyChangeZero)
        {
            //setup
            var iexResponse = new IexResponse
            {
                quote = new IexResponseQuote
                {
                    symbol = "AAPL",
                    change = null,
                    calculationPrice = calculationType,
                    companyName = "Apple",
                    latestPrice = latestPrice,
                    open = 210.75M,
                    peRatio = 30.45M,
                    week52High = 250.10564M,
                    week52Low = 160.75645M,
                    changePercent = 0.20M,
                    sector = "Technology",
                    ytdChange = 0.0987540M,
                    close = close
                }
            };
            var mapper = new IexMapper();

            //execute
            var stock = mapper.MapIexResponseToStock(iexResponse);

            //validate
            Assert.Equal(iexResponse.quote.symbol, stock.Ticker);
            Assert.Equal(iexResponse.quote.companyName, stock.Company);
            Assert.Equal(iexResponse.quote.sector, stock.Sector);
            Assert.Equal(Math.Round(price, 2), stock.Price);
            Assert.Equal(Math.Round(iexResponse.quote.week52High.Value, 2), stock.Week52High);
            Assert.Equal(Math.Round(iexResponse.quote.week52Low.Value, 2), stock.Week52Low);
            Assert.False(stock.DailyPercentageChange == 0);
            Assert.False(stock.DailyChange == 0);
            Assert.Equal(dailyChangeZero, stock.DailyPercentageChangeSinceLastClose == 0);
            Assert.Equal(dailyChangeZero, stock.DailyChangeSinceLastClose == 0);
            Assert.True(DateTime.UtcNow.Subtract(stock.LastUpdated).TotalSeconds < 20);
            Assert.Equal(Math.Round(iexResponse.quote.ytdChange.Value, 4), stock.YearToDateChange);
        }
    }
}