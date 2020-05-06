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
        private IMapper _mapper;
        private HttpClient _client;
        private Dictionary<string, Stock> _stockCache;
        private readonly int _cacheTimeSeconds = 120;

        public StockRetriever(HttpClient client, IMapper mapper)
        {
            Initialize(client, mapper);
        }

        public StockRetriever()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.iextrading.com/1.0/");
            client.DefaultRequestHeaders.Accept.Add
            (
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            Initialize(client, new Mapper());
        }

        private void Initialize(HttpClient client, IMapper mapper)
        {
            _mapper = mapper;
            _client = client;
            _stockCache = new Dictionary<string, Stock>();
        }

        public async Task<Stock> GetStockAsync(string ticker)
        {
            try
            {
                if (_stockCache.TryGetValue(ticker.ToUpper(), out var cachedStock))
                {
                    if (cachedStock != null && DateTime.UtcNow.Subtract(cachedStock.LastUpdated).TotalSeconds < _cacheTimeSeconds)
                    {
                        return cachedStock;
                    }
                }

                HttpResponseMessage task = await _client.GetAsync($"stock/{ticker}/book");
                string jsonString = await task.Content.ReadAsStringAsync();
                IexResponse response = JsonConvert.DeserializeObject<IexResponse>(jsonString);

                var newStock = _mapper.MapIexResponseToStock(response);

                if(_stockCache.ContainsKey(ticker.ToUpper()))
                {
                    _stockCache[ticker.ToUpper()] = newStock;
                }
                else
                { 
                    _stockCache.Add(ticker.ToUpper(), newStock);
                }
                return newStock;
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