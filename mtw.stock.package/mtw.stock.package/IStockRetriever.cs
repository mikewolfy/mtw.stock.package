namespace Emptywolf.Stocks
{
    public interface IStockRetriever
    {
        Stock GetStock(string ticker);
    }
}