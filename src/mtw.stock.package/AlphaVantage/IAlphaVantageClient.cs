using Emptywolf.Stocks.AlphaVantage.Models;
using System.Threading.Tasks;

namespace Emptywolf.Stocks.AlphaVantage
{
    public interface IAlphaVantageClient
    {
        Task<OverviewResponseDto> GetStock(string ticker);
    }
}