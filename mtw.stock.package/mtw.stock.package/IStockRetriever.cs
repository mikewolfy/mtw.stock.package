using System.Threading.Tasks;

namespace Emptywolf.Stocks
{
    public interface IStockRetriever
    {
        Task<Stock> GetStockAsync(string ticker);
    }
}