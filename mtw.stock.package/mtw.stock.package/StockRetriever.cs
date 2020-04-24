namespace Emptywolf.Stocks
{
    public class StockRetriever: IStockRetriever
    {
        public Stock GetStock(string ticker)
        {
            return new Stock { Ticker = ticker };
        }
    }
}
