

using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.Common.DAL;
using STEM_ROBOT.DAL.Models;
using System.Linq.Expressions;

public class GenericRep<T> : IGenericRep<T> where T : class
{
    protected StemdbContext _context;
    protected DbSet<T> _dbSet;

    public GenericRep(StemdbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();  // Thêm SaveChanges để cập nhật vào DB
    }

    public virtual IEnumerable<T> All(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return query.ToList();
    }



    public void Delete(int id)
    {
        T delete = _dbSet.Find(id);
        if (delete != null)
        {
            _dbSet.Remove(delete);
            _context.SaveChanges();  // Thêm SaveChanges để cập nhật vào DB
        }
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).ToList();
    }

    
    public T getID(object id)
    {
        return _dbSet.Find(id);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();  // Thêm SaveChanges để cập nhật vào DB
    }
}
