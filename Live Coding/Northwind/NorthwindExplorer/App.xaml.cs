using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace NorthwindExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            this.Configuration = builder.Build();

            Console.WriteLine(Configuration.GetConnectionString("NorthwindConString"));

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();


            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // TODO: Connectionstring in appsettings.json anpassen und hier einfügen
            services.AddDbContext<NorthwindDal.Model.NorthwindContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
            services.AddTransient(typeof(MainWindow));
        }
    }
}
