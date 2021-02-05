using Emptywolf.Stocks.AlphaVantage.Models;
using Emptywolf.Stocks.Constants;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Emptywolf.Stocks.AlphaVantage
{
    public class AlphaVantageClient: IAlphaVantageClient
    {
        private HttpClient _client;
        private string _key;
        private Dictionary<string, CachedStock> _stockCache;
        private readonly int _cacheTimeSeconds = 1200;

        public AlphaVantageClient(HttpClient client, IConfiguration config)
        {
            _key = config[AlphaVantageConfiguration.AlphaVantageApiKey];
            _client = client;
            _stockCache = new Dictionary<string, CachedStock>();
        }

        public AlphaVantageClient(string key)
        {
            _key = key;
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://www.alphavantage.co/");
            _stockCache = new Dictionary<string, CachedStock>();
        }

        public async Task<OverviewResponseDto> GetStockOverview(string ticker)
        {
            if (_stockCache.TryGetValue(ticker.ToUpper(), out var cachedStock))
            {
                if (cachedStock != null && DateTime.UtcNow.Subtract(cachedStock.LastUpdated).TotalSeconds < _cacheTimeSeconds)
                {
                    return cachedStock.Dto;
                }
            }

            HttpResponseMessage task = await _client.GetAsync($"/query?function=OVERVIEW&symbol={ticker}&apikey={_key}");
            string jsonString = await task.Content.ReadAsStringAsync();
            var stockDto = JsonConvert.DeserializeObject<OverviewResponseDto>(jsonString);

            var newCache = new CachedStock { Dto = stockDto, LastUpdated = DateTime.Now };
            if (_stockCache.ContainsKey(ticker.ToUpper()))
            {
                _stockCache[ticker.ToUpper()] = newCache;
            }
            else
            {
                _stockCache.Add(ticker.ToUpper(), newCache);
            }

            return stockDto;
        }

        private class CachedStock
        {
            public OverviewResponseDto Dto { get; set; }
            public DateTime LastUpdated { get; set; }
        }
    }
}
