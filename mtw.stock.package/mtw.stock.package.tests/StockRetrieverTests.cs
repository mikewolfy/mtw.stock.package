using Emptywolf.Stocks;
using System;
using Xunit;

namespace mtw.stock.package.tests
{
    public class StockRetrieverTests
    {
        [Fact]
        public void Test()
        {
            //setup
            var ticker = "AAPL";
            var retriever = new StockRetriever();

            //execute
            var result = retriever.GetStock(ticker);

            //validate
            Assert.NotNull(result);
            Assert.Equal(ticker, result.Ticker);
        }
    }
}
