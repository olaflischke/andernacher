namespace HistoricalRatesDal
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public double Rate { get; set; }
        public TradingDay TradingDay { get; set; }
        public bool TradingInterupted { get; set; }
    }
}