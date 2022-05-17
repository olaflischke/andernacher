using Microsoft.EntityFrameworkCore;
using NorthwindDal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDal.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Insert(TEntity entity);

        TEntity GetById(object id);

        IEnumerable<TEntity> GetAll();

        // Filtern (Where...)
        // Sortieren (OrderBy...)
        // Include
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                    string includeProperties = "");
    }

    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        NorthwindContext context;
        DbSet<TEntity> dbSet;

        public GenericRepository(NorthwindContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, 
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
                                        string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy!=null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        [Obsolete("Use Get() instead.")]
        public IEnumerable<TEntity> GetAll()
        {
            return this.dbSet;
        }

        public TEntity GetById(object id)
        {
            return this.dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            this.context.Add(entity);
        }

        public void Update(TEntity entity)
        {
            this.dbSet.Attach(entity);
            this.context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            this.dbSet.Remove(entity);
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = this.dbSet.Find(id);
            Delete(entityToDelete);
        }
    }
}
