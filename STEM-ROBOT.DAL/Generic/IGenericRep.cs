using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.DAL
{
    public interface IGenericRep<T> where T : class
    {
        IEnumerable<T> All(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          string includeProperties = "",
          int? pageIndex = null,
          int? pageSize = null);
        T getID(object id);
        void Delete(int id);
        void Add(T entity);
        void Update(T entity);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}