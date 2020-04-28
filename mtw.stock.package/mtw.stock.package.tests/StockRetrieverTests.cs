using Emptywolf.Stocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace mtw.stock.package.tests
{
    public class StockRetrieverTests
    {
        [Fact]
        public async Task Test()
        {
            //setup
            var ticker = "AAPL";
            var retriever = new StockRetriever();

            //execute
            var result = await retriever.GetStockAsync(ticker);

            //validate
            Assert.NotNull(result);
            Assert.Equal(ticker, result.Ticker);
        }
    }
}