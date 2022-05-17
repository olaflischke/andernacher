using Microsoft.EntityFrameworkCore;
using NorthwindDal;
using NorthwindDal.Model;
using NorthwindDal.Repository;
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

        private UnitOfWork unitOfWork;

        public MainWindowViewModel()
        {
            this.AddCustomer = new RelayCommand(p => CanAddCustomer(), a => AddCustomerAction());
            this.EditCustomer = new RelayCommand(p => CanEditCustomer(), a => EditCustomerAction());

            this.unitOfWork = UnitOfWork.Create(); //new UnitOfWork();
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
                try
                {
                    unitOfWork.CustomerRepository.Update(this.SelectedCustomer);
                    unitOfWork.Save();

                }
                catch (NorthwindDalException ex)
                {
                    if (ex.InnerException is DbUpdateConcurrencyException)
                    {
                        // TODO: Meldung an User etc.
                        // Customer neu laden
                        this.SelectedCustomer = unitOfWork.CustomerRepository.GetById(this.SelectedCustomer.CustomerId);
                    }
                }

                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                this.SelectedCustomer = unitOfWork.CustomerRepository.GetById(this.SelectedCustomer.CustomerId);
            }
        }

        private bool CanAddCustomer()
        {
            return true;
        }

        private void AddCustomerAction()
        {
            Customer customer = new Customer();
            AddEditCustomer addCustomer = new AddEditCustomer(customer);

            if (addCustomer.ShowDialog() == true)
            {
                unitOfWork.CustomerRepository.Insert(customer);
                unitOfWork.Save();
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
