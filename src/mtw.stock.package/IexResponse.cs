namespace Emptywolf.Stocks
{
    public class IexResponse
    {
        public IexResponseQuote quote { get; set; }
    }

    public class IexResponseQuote
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string sector { get; set; }
        public string calculationPrice { get; set; }
        public decimal? latestPrice { get; set; }
        public decimal? open { get; set; }
        public decimal? peRatio { get; set; }
        public decimal? week52High { get; set; }
        public decimal? week52Low { get; set; }
        public decimal? change { get; set; }
        public decimal? changePercent { get; set; }
        public decimal? ytdChange { get; set; }
        public decimal? close { get; set; }
    }
}