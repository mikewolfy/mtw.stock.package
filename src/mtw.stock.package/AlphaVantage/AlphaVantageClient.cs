using Emptywolf.Stocks.AlphaVantage.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Emptywolf.Stocks.AlphaVantage
{
    public class AlphaVantageClient: IAlphaVantageClient
    {
        private HttpClient _client;
        private string _key;

        public AlphaVantageClient(HttpClient client, AlphaVantageConfiguration config)
        {
            _key = config.ApiKey;
            _client = client;
        }

        public AlphaVantageClient(string key)
        {
            _key = key;
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://www.alphavantage.co/");
        }

        public async Task<OverviewResponseDto> GetStock(string ticker)
        {
            HttpResponseMessage task = await _client.GetAsync($"/query?function=OVERVIEW&symbol={ticker}&apikey={_key}");
            string jsonString = await task.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OverviewResponseDto>(jsonString);
        }
    }
}
