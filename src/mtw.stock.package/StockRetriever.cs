using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Emptywolf.Stocks
{
    public class StockRetriever: IStockRetriever
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        private readonly Dictionary<string, Stock> _stockCache;
        private readonly int _cacheTimeMinutes = 5;

        public StockRetriever(HttpClient client, IMapper mapper)
        {
            _mapper = mapper;
            _client = client;
            _stockCache = new Dictionary<string, Stock>();
        }

        public StockRetriever()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://api.iextrading.com/1.0/");
            _client.DefaultRequestHeaders.Accept.Add
            (
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async Task<Stock> GetStockAsync(string ticker)
        {
            try
            {
                HttpResponseMessage task = await _client.GetAsync($"stock/{ticker}/book");
                string jsonString = await task.Content.ReadAsStringAsync();
                IexResponse response = JsonConvert.DeserializeObject<IexResponse>(jsonString);
                return _mapper.MapIexResponseToStock(response);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Stock>> GetStocksAsync(string[] tickers)
        {
            var stocks = new List<Stock>();
            var tasks = tickers.Select(t => GetStockAsync(t));
            var results = await Task.WhenAll(tasks);

            results.ToList().ForEach(r =>
            {
                if (r != null)
                {
                    stocks.Add(r);
                }
            });

            return stocks.OrderBy(s => s.Ticker).ToList();
        }
    }
}