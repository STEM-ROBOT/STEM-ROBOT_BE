using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.DAL
{
    public class GenericRep<T> : IGenericRep<T> where T : class
    {
        //protected DbContext _context;
        //protected DbSet<T> _dbSet;

        //public GenericRep()
        //{
        //    _dbSet = _context.Set<T>();
        //}
        //public void Add(T entity)
        //{
        //    _dbSet.Add(entity);
        //}

        //public IEnumerable<T> All(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null)
        //{
        //    IQueryable<T> query = _dbSet;

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    foreach (var includeProperty in includeProperties.Split
        //        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty);
        //    }

        //    if (orderBy != null)
        //    {
        //        query = orderBy(query);
        //    }

        //    if (pageIndex.HasValue && pageSize.HasValue)
        //    {
        //        int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
        //        int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;

        //        query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
        //    }

        //    return query.ToList();
        //}

        //public void Delete(int id)
        //{
        //    T delete = _dbSet.Find(id);
        //    if (delete != null)
        //    {
        //        _dbSet.Remove(delete);
        //        _context.SaveChanges();
        //    }
        //}

        //public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        //{

        //    return _dbSet.Where(predicate).ToList();
        //}

        //public T getID(object id)
        //{
        //    return _dbSet.Find(id);
        //}

        //public void Update(T entity)
        //{
        //    _dbSet.Update(entity);
        //}
        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> All(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T getID(object id)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
