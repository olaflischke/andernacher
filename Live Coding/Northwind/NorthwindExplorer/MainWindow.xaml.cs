using NorthwindDal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using NorthwindDal.Repository;

namespace NorthwindExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //NorthwindContext context = new NorthwindContext(); // Hier besser nicht!
        MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // select distinct country from Customers
            UnitOfWork unitOfWork = UnitOfWork.Create();  //new UnitOfWork())
                                                          //var qCountries = northwindContext.Customers.Select(x => GetCountryFromCustomer(x)).Distinct();
            var qCountries = unitOfWork.CustomerRepository.Get().Select(x => x.Country).Distinct();

            // LINQ: Deferred Execution - Query wird erst ausgeführt bei Zugriff auf die Ergebnismenge (hier: qCountries)
            foreach (string item in qCountries)
            {
                TreeViewItem treeViewItem = new TreeViewItem() { Header = item };
                treeViewItem.Items.Add(new TreeViewItem());
                treeViewItem.Expanded += this.TreeViewItem_Expanded;
                trvCustomers.Items.Add(treeViewItem);
            }

        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem land)
            {
                string country = land.Header?.ToString();
                land.Items.Clear();

                if (country != null)
                {
                    UnitOfWork unitOfWork = UnitOfWork.Create();
                    var qCustomersByThisCountry = unitOfWork.CustomerRepository.Get(cu => cu.Country == country)
                                                                    .Select(cu => new { cu.CompanyName, cu.CustomerId });

                    foreach (var item in qCustomersByThisCountry)
                    {
                        TreeViewItem tviCustomer = new TreeViewItem() { Header = item.CompanyName, Tag = item.CustomerId };
                        tviCustomer.Selected += this.TreeViewCustomer_Selected;

                        land.Items.Add(tviCustomer);
                    }

                }
            }
        }

        private void TreeViewCustomer_Selected(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem customerNode)
            {
                string customerId = customerNode.Tag.ToString();

                // TODO: UnitOfWork hier auch
                using (NorthwindContext context = new NorthwindContext())
                {

                    //Customer customer = context.Customers.Where(cu => cu.CustomerId == customerId).FirstOrDefault();
                    //Customer customer = context.Customers.FirstOrDefault(customer => customer.CustomerId == customerId);    
                    //Customer customer = context.Customers.Find(customerId); // Voraussetzung: Primärschlüssel!
                    Customer customer = context.Customers.Include(cu => cu.Orders)
                                                        .AsNoTracking()
                                                        .FirstOrDefault(cu => cu.CustomerId == customerId);

                    viewModel.SelectedCustomer = customer;
                }
            }
        }

        private string GetCountryFromCustomer(Customer customer)
        {
            return customer.Country;
        }
    }
}
