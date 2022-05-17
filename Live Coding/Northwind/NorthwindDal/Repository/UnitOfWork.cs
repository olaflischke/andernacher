using Microsoft.EntityFrameworkCore;
using NorthwindDal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDal.Repository
{
    public class UnitOfWork : IDisposable
    {
        NorthwindContext context = new NorthwindContext();

        #region Singleton-Implementierung
        // Hinweis: Singleton kann Performance kosten
        private static UnitOfWork _instance;

        private UnitOfWork()
        {

        }

        public static UnitOfWork Create()
        {
            return _instance ?? (_instance = new UnitOfWork());
        }
        #endregion


        private GenericRepository<Customer> _customerRepo;

        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (_customerRepo == null)
                {
                    _customerRepo = new GenericRepository<Customer>(context);
                }
                return _customerRepo;
            }
        }

        private GenericRepository<Order> _orderRepo;

        public GenericRepository<Order> OrderRepository
        {
            get { return _orderRepo ?? (_orderRepo = new GenericRepository<Order>(context)); }
        }

        private GenericRepository<OrderDetail> _orderDetailRepo;

        public GenericRepository<OrderDetail> OrderDetailRepository
        {
            get { return _orderDetailRepo ?? (_orderDetailRepo = new GenericRepository<OrderDetail>(context)); }
        }

        private GenericRepository<Product> _productsRepo;

        public GenericRepository<Product> ProductsRepository
        {
            get { return _productsRepo ?? (_productsRepo = new GenericRepository<Product>(context)); }
        }

        public void Save()
        {
            try
            {
                this.context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new NorthwindDalException("Daten in der DB neuer als Deine!", ex);
            }
        }

        #region IDisposable
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

}

