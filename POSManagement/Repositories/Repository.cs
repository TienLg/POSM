using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace POSManagement.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> dbSet;

        public Repository(DbContext dataContext)
        {
            dbSet = dataContext.Set<T>();
        }

        #region IRepository<T> members
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        #endregion
    }
}
