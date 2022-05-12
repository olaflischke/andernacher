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
using System.Windows.Shapes;

namespace NorthwindExplorer
{
    /// <summary>
    /// Interaction logic for AddEditCustomer.xaml
    /// </summary>
    public partial class AddEditCustomer : Window
    {
        public AddEditCustomer(Customer customer)
        {
            InitializeComponent();
            this.Customer = customer;
            this.DataContext = this.Customer;    
        }

        public Customer Customer { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
