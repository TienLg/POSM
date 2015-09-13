using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POSManagement.Repositories
{
    public interface IRepository<T>
    {
         void Add(T entity);
         void Remove(T entity);
         IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
         T GetById(int id);
    }
}
