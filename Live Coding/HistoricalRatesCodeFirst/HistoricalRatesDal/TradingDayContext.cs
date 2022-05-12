using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoricalRatesDal
{
    public class TradingDayContext : DbContext
    {
        public TradingDayContext()
        {

        }

        public TradingDayContext(DbContextOptions options):base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TradingDayData;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }

            
        }

        public DbSet<TradingDay> TradingDays { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
    }
}
