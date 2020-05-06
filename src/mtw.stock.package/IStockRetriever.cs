using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emptywolf.Stocks
{
    public interface IStockRetriever
    {
        Task<Stock> GetStockAsync(string ticker);

        Task<IEnumerable<Stock>> GetStocksAsync(string[] tickers);
    }
}