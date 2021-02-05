using Emptywolf.Stocks.AlphaVantage;
using System.Threading.Tasks;
using Xunit;

namespace mtw.stock.package.tests.AlphaVantage
{
    public class AlphaVantageClientTests
    {
        [Fact]
        public async Task TestDefaultClient()
        {
            //setup
            var ticker = "IBM";
            var retriever = new AlphaVantageClient("demo");

            //execute
            var result = await retriever.GetStock(ticker);

            //validate
            Assert.NotNull(result);
            Assert.Equal(ticker, result.Symbol);
        }
    }
}