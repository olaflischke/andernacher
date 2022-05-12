using Microsoft.EntityFrameworkCore;
using NorthwindDal.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindExplorer
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            this.AddCustomer = new RelayCommand(p => CanAddCustomer(), a => AddCustomerAction());
            this.EditCustomer = new RelayCommand(p => CanEditCustomer(), a => EditCustomerAction());
        }

        private bool CanEditCustomer()
        {
            if (SelectedCustomer == null)
            {
                return false;
            }
            return true;
        }

        private void EditCustomerAction()
        {
            AddEditCustomer editCustomer = new AddEditCustomer(this.SelectedCustomer);
            if (editCustomer.ShowDialog() == true)
            {
                using (NorthwindContext context = new NorthwindContext())
                {
                    try
                    {
                        // INSERT!
                        //context.Add(editCustomer.Customer);

                        // Manuelles Attachen, State setzen
                        //context.Attach(editCustomer.Customer);
                        //context.Entry(editCustomer.Customer).State = EntityState.Modified;

                        // Update(): Attach + State=Modified, Achtung: Abhängige Elemente ggf. auch Modified
                        context.Update(editCustomer.Customer);

                        context.SaveChanges();

                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        // Benutzer informieren

                        // Nicht speichern, Änderungen verwerfen (Database wins)
                        context.Entry(this.SelectedCustomer).Reload();

                        // oder
                        // Doch speichern (Client wins)
                        context.Entry(this.SelectedCustomer).OriginalValues.SetValues(context.Entry(this.SelectedCustomer).GetDatabaseValues());
                        context.SaveChanges();
                    }
                }
            }
            else
            {
                using (NorthwindContext context = new NorthwindContext())
                {
                    // Customer aus der DB aktualisieren
                    context.Entry(this.SelectedCustomer).Reload();

                    // ReLoad macht etwa dieses hier:
                    //context.Entry(this.SelectedCustomer).CurrentValues.SetValues(context.Entry(this.SelectedCustomer).GetDatabaseValues());
                    //context.Entry(this.SelectedCustomer).State = EntityState.Unchanged;


                    // Customer im Speicher zurücksetzen (nur bei langlebigen Contexten!)
                    //context.Entry(this.SelectedCustomer).CurrentValues.SetValues(context.Entry(this.SelectedCustomer).OriginalValues);
                }
            }
        }

        private bool CanAddCustomer()
        {
            return true;
        }

        private void AddCustomerAction()
        {
            Customer customer = new Customer();
            AddEditCustomer addCustomer=new AddEditCustomer(customer);

            if (addCustomer.ShowDialog() == true)
            {
                using (NorthwindContext context = new NorthwindContext())
                {
                    context.Customers.Add(customer);
                    context.SaveChanges();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                //using (NorthwindContext context = new NorthwindContext())
                //{
                //    _selectedCustomer.Orders = context.Orders.Where(od => od.CustomerId == _selectedCustomer.CustomerId).ToList();
                //}
                OnPropertyChanged();
            }
        }

        public RelayCommand AddCustomer { get; set; }
        public RelayCommand EditCustomer { get; set; }
    }
}
