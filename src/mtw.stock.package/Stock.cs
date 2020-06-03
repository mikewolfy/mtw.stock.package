using System;

namespace Emptywolf.Stocks
{
    public class Stock
    {
        public string Company { get; set; }
        public string Sector { get; set; }
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public decimal PE { get; set; }
        public decimal Eps { get; set; }
        public decimal Week52High { get; set; }
        public decimal Week52Low { get; set; }
        public decimal DailyChange { get; set; }
        public decimal DailyPercentageChange { get; set; }
        public decimal YearToDateChange { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
