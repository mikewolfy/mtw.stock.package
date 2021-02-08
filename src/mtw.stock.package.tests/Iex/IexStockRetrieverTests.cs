using Emptywolf.Stocks;
using Emptywolf.Stocks.Iex;
using Emptywolf.Stocks.Iex.Models;
using Moq;
using mtw.stock.package.tests.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace mtw.stock.package.tests.Iex
{
    public class IexStockRetrieverTests
    {
        Mock<IIexMapper> _mockIexMapper;

        public IexStockRetrieverTests()
        {
            _mockIexMapper = new Mock<IIexMapper>();
        }

        [Fact]
        public async Task TestDefaultClient()
        {
            //setup
            var ticker = "AAPL";
            var retriever = new IexStockRetriever();

            //execute
            var result = await retriever.GetStockAsync(ticker);

            //validate
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestHttpClientMockedMapper()
        {
            //setup
            SetupMocks();
            var ticker = "AAPL";
            var path = $"/stock/{ticker}/book";
            var mockResponse = GetIexResponse();

            HttpResponseMessage mockHttpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockResponse), Encoding.UTF8, "application/json")
            };
            var client = GetMockClient(new MockClientConfigurator
            {
                MessagesToReturn = new Dictionary<string, HttpResponseMessage> { { path, mockHttpResponse } }
            });
            var retriever = new IexStockRetriever(client, _mockIexMapper.Object);

            //execute
            var result = await retriever.GetStockAsync(ticker);

            //validate
            Assert.NotNull(result);
            _mockIexMapper.Verify(m => m.MapIexResponseToStock(It.IsAny<IexResponse>()), Times.Once);
        }

        [Fact]
        public async Task TestHttpCollections()
        {
            //setup
            SetupMocks();
            var ticker = "AAPL";
            var path = $"/stock/{ticker}/book";

            var ticker2 = "MSFT";
            var path2 = $"/stock/{ticker2}/book";

            var mockResponse = GetIexResponse();

            HttpResponseMessage mockHttpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockResponse), Encoding.UTF8, "application/json")
            };

            mockResponse.quote.symbol = "MSFT";
            HttpResponseMessage mockHttpResponse2 = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockResponse), Encoding.UTF8, "application/json")
            };


            var client = GetMockClient(new MockClientConfigurator
            {
                MessagesToReturn = new Dictionary<string, HttpResponseMessage> { 
                    { path, mockHttpResponse } ,
                    { path2, mockHttpResponse2 }
                }
            });
            var retriever = new IexStockRetriever(client, _mockIexMapper.Object);

            //execute
            var result = await retriever.GetStocksAsync(new string[] { "AAPL", "MSFT" });

            //validate
            Assert.NotNull(result);
            Assert.Equal(ticker, result.ToList()[0].Ticker);
            Assert.Equal(ticker2, result.ToList()[1].Ticker);
            _mockIexMapper.Verify(m => m.MapIexResponseToStock(It.IsAny<IexResponse>()), Times.Exactly(2));
        }

        [Fact]
        public async Task TestCaching()
        {
            //setup
            var ticker = "AAPL";
            var path = $"/stock/{ticker}/book";

            var mockResponse = GetIexResponse();

            HttpResponseMessage mockHttpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockResponse), Encoding.UTF8, "application/json")
            };


            var client = GetMockClient(new MockClientConfigurator
            {
                MessagesToReturn = new Dictionary<string, HttpResponseMessage> {
                    { path, mockHttpResponse }
                }
            });
            var retriever = new IexStockRetriever(client, new IexMapper());

            //execute
            var result = await retriever.GetStockAsync("AAPL");
            await Task.Delay(1100);
            var result2 = await retriever.GetStockAsync("AAPL");

            //validate
            Assert.NotNull(result);
            Assert.NotNull(result2);
            Assert.Equal(result.LastUpdated, result2.LastUpdated);
        }

        private void SetupMocks()
        {
            _mockIexMapper.Setup(m => m.MapIexResponseToStock(It.IsAny<IexResponse>()))
                .Returns((IexResponse r) => new Stock() { Ticker = r.quote.symbol });
        }

        private HttpClient GetMockClient(MockClientConfigurator config)
        {
            var mockMessageHandler = new MockHttpMessageHandler(config.MessagesToReturn);

            return new HttpClient(mockMessageHandler)
            {
                BaseAddress = new Uri("https://api.iextrading.com")
            };
        }

        private class MockClientConfigurator
        {
            public Dictionary<string, HttpResponseMessage> MessagesToReturn { get; set; }
        }

        private IexResponse GetIexResponse()
        {
            return new IexResponse
            {
                quote = new IexResponseQuote
                {
                    symbol = "AAPL",
                    change = null,
                    companyName = "Apple",
                    latestPrice = 215.50M,
                    open = 210.75M,
                    peRatio = 30.45M,
                    week52High = 250.10M,
                    week52Low = 160.75M,
                    changePercent = 0.20M,
                    sector = "Technology"
                }
            };
        }
    }
}