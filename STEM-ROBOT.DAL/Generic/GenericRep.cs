

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

    public virtual IEnumerable<T> All(
    Expression<Func<T, bool>> filter = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    string includeProperties = "",
    int? pageIndex = null,
    int? pageSize = null)
    {
        IQueryable<T> query = _dbSet;

        // Áp dụng bộ lọc (nếu có)
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Thêm các thuộc tính liên kết (include), sử dụng cho các thực thể có liên quan
        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        // Sắp xếp (nếu có)
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // Phân trang (nếu có)
        if (pageIndex.HasValue && pageSize.HasValue)
        {
            query = query.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
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


    public T GetById(object id)
    {
        return _dbSet.Find(id);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();  // Thêm SaveChanges để cập nhật vào DB
    }
}