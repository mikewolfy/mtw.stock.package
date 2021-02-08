using System;

namespace Emptywolf.Stocks.Models
{
    public class PriceDetail
    {
        public decimal? latestPrice { get; set; }
        public decimal? change { get; set; }
        public decimal? changePercent { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
