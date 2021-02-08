using Emptywolf.Stocks.Iex.Models;

namespace Emptywolf.Stocks
{
    public interface IIexMapper
    {
        Stock MapIexResponseToStock(IexResponse response);
    }
}