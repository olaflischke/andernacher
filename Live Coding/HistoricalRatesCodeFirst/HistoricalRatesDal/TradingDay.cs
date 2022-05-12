using System.Globalization;
using System.Xml.Linq;

namespace HistoricalRatesDal
{
    public class TradingDay
    {
        public TradingDay()
        {

        }

        public TradingDay(XElement tradingDayNode)
        {
            this.Date = Convert.ToDateTime(tradingDayNode.Attribute("time").Value);

            var qRates = tradingDayNode.Elements().Select(el => new ExchangeRate()
            {
                Symbol = el.Attribute("currency").Value,
                Rate = Convert.ToDouble(el.Attribute("rate").Value, NumberFormatInfo.InvariantInfo),
                TradingDay = this
            });

            this.ExchangeRates = qRates.ToList();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<ExchangeRate> ExchangeRates { get; set; }
        public string Location { get; set; }
    }
}