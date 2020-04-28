using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Emptywolf.Stocks
{
    public class StockRetriever: IStockRetriever
    {
        private readonly HttpClient client;

        public StockRetriever()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.iextrading.com/1.0/");
            client.DefaultRequestHeaders.Accept.Add
            (
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async Task<Stock> GetStockAsync(string ticker)
        {
            try
            {
                HttpResponseMessage task = await client.GetAsync($"stock/{ticker}/book");
                string jsonString = await task.Content.ReadAsStringAsync();
                IexResponse response = JsonConvert.DeserializeObject<IexResponse>(jsonString);

                decimal price = response.quote.latestPrice.HasValue ? response.quote.latestPrice.Value : 0;
                decimal open = response.quote.open.HasValue ? response.quote.open.Value : 0;
                decimal change = price - open;
                decimal percentageChange = price == 0 ? 0 : change / price * 100;

                var stock = new Stock()
                {
                    Ticker = ticker,
                    Company = response.quote.companyName,
                    Price = price,
                    PE = response.quote.peRatio.HasValue ? response.quote.peRatio.Value : 0,
                    Week52High = response.quote.week52High.HasValue ? response.quote.week52High.Value : 0,
                    Week52Low = response.quote.week52Low.HasValue ? response.quote.week52Low.Value : 0,
                    Sector = response.quote.sector,
                    DailyChange = change,
                    DailyPercentageChange = percentageChange
                };
                stock.Eps = stock.PE != 0 ? stock.Price / stock.PE : 0;
                return stock;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private class IexResponse
        {
            public IexResponseQuote quote { get; set; }
        }

        private class IexResponseQuote
        {
            public string symbol { get; set; }
            public string companyName { get; set; }
            public string sector { get; set; }
            public decimal? latestPrice { get; set; }
            public decimal? open { get; set; }
            public decimal? peRatio { get; set; }
            public decimal? week52High { get; set; }
            public decimal? week52Low { get; set; }
            public decimal? change { get; set; }
            public decimal? changePercent { get; set; }
        }
    }
}
