namespace Emptywolf.Stocks
{
    public interface IMapper
    {
        Stock MapIexResponseToStock(IexResponse response);
    }
}