using HistoricalRatesDal;
using NUnit.Framework;
using System;
using System.Linq;

namespace HistoricalRatesDalUnitTests
{
    public class ArchiveTests
    {
        string url;

        [SetUp]
        public void Setup()
        {
            url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";
        }

        [Test]
        public void IsArchiveInitializing()
        {
            Archive archive = new Archive(url);

            Console.WriteLine($"{archive.TradingDays.FirstOrDefault().Date:d}: {archive.TradingDays.FirstOrDefault().ExchangeRates.FirstOrDefault().Rate} ");

            Assert.AreEqual(GetCountOfAttribute("time"), archive.TradingDays.Count);
        }

        private int GetCountOfAttribute(string attributeName)
        {
            return 62;
        }

        [Test]
        public void IsArchiveSaving()
        {
            Archive archive = new Archive(url);
            archive.SaveToDb();
        }
    }
}