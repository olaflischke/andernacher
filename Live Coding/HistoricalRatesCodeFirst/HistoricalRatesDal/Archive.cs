using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HistoricalRatesDal
{
    public class Archive
    {
        public Archive(string url)
        {
            this.TradingDays = GetData(url);
        }

        private List<TradingDay>? GetData(string url)
        {
            XDocument document = XDocument.Load(url);

            var qDays = document.Root.Descendants()
                        .Where(nd => nd.Name.LocalName == "Cube" && nd.Attributes().Any(at => at.Name == "time"))
                        .Select(nd => new TradingDay(nd));

            return qDays.ToList();
        }

        public void SaveToDb()
        {
            DbContextOptions<TradingDayContext> options = new DbContextOptionsBuilder<TradingDayContext>()
                .UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = TradingDayData; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False").Options;

            using (TradingDayContext context = new TradingDayContext(options))
            {
                context.Database.EnsureCreated();
                

                context.AddRange(this.TradingDays);
                context.SaveChanges();
            }
        }

        public List<TradingDay> TradingDays { get; set; }
    }
}
